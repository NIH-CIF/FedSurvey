using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;
using Microsoft.AspNetCore.Http;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FedSurvey.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public UploadController(CoreDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Upload([FromForm] UploadModel uploadModel)
        {
            if (uploadModel.file == null || uploadModel.key == null)
            {
                return UnprocessableEntity();
            }

            // Starting by assuming 2016 thru 2019 format.
            // 2020 will have to be programmed for specially anyway.
            using (var stream = uploadModel.file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // We will store the execution we are attaching to far outside so
                    // that we only load it once.
                    Execution execution = null;

                    do
                    {
                        // Later consideration: is doing n queries where n is the number of sheets okay?
                        QuestionTypeString savedString = _context.QuestionTypeStrings.Where(qts => qts.Name == reader.Name).Include(x => x.QuestionType).FirstOrDefault();
                        QuestionType currentType = savedString != null ? savedString.QuestionType : null;

                        if (currentType != null)
                        {
                            // Pull out a PossibleResponse mapping from the header row.
                            reader.Read();

                            const int POSSIBLE_RESPONSE_START = 5;
                            Dictionary<int, PossibleResponse> colToPossibleResponse = new Dictionary<int, PossibleResponse>();

                            for (int i = POSSIBLE_RESPONSE_START; i < reader.FieldCount - 1; i++)
                            {
                                // Long-term, might be good to support data in fields with "N" suffix.
                                string response = Regex.Replace(reader.GetString(i).Replace("\n", " "), @"[%|N]$", "").Trim();

                                PossibleResponseString responseName = _context.PossibleResponseStrings.Where(prs => prs.Name == response).Include(x => x.PossibleResponse).FirstOrDefault();

                                if (responseName == null)
                                {
                                    System.Diagnostics.Debug.WriteLine(response + " causing trouble");
                                    return UnprocessableEntity();
                                }
                                else
                                {
                                    colToPossibleResponse[i] = responseName.PossibleResponse;
                                }

                                // Hiding this for now, as I should have seeded the question type properly.
                                //if (possibleResponse == null)
                                //{
                                //    PossibleResponse newPossibleResponse = new PossibleResponse
                                //    {
                                //        QuestionType = currentType
                                //    };
                                //}
                                //else
                                //{

                                //}
                            }

                            // If we do not have an execution, we need to set it or make one.
                            if (execution == null)
                            {
                                execution = _context.Executions.Where(e => e.Key == uploadModel.key).FirstOrDefault();

                                if (execution == null)
                                {
                                    Execution newExecution = new Execution
                                    {
                                        Key = uploadModel.key,
                                        Notes = uploadModel.notes
                                    };
                                    _context.Executions.Add(newExecution);
                                    execution = newExecution;

                                    // Save changes so that execution has an ID for comparison later.
                                    // Can this be refactored out?
                                    _context.SaveChanges();
                                }
                            }

                            System.Diagnostics.Debug.WriteLine("Processing " + reader.Name);

                            // Store the organizations, question executions that have been made to not double create.
                            Dictionary<string, DataGroupString> organizationStringToObject = new Dictionary<string, DataGroupString>();
                            Dictionary<string, QuestionExecution> questionTextToObject = new Dictionary<string, QuestionExecution>();

                            while (reader.Read())
                            {
                                // Columns:
                                // 0 Sorting Level - this will be ignored unless it is later needed
                                // 1 Organization - this is the Data Group
                                // 2 Item - this is the question position once you take off the leading Q
                                // 3 Item Text - this is the question text in this year
                                // 4 Item Respondents - this is the number that will be multiplied by the percentage to get Count
                                // 5-n Options - each column becomes a possible response option
                                string rowOrgName = reader.GetString(1);
                                DataGroupString organizationName = organizationStringToObject.ContainsKey(rowOrgName) ? organizationStringToObject[rowOrgName] : _context.DataGroupStrings.Where(dgs => dgs.Name == rowOrgName).Include(x => x.DataGroup).FirstOrDefault();
                                DataGroup organization = organizationName != null ? organizationName.DataGroup : null;

                                // Make new organization if one does not already exist.
                                if (organization == null)
                                {
                                    DataGroup newOrganization = new DataGroup { };
                                    DataGroupString newString = new DataGroupString
                                    {
                                        DataGroup = newOrganization,
                                        Name = rowOrgName
                                    };

                                    _context.DataGroups.Add(newOrganization);
                                    _context.DataGroupStrings.Add(newString);

                                    organization = newOrganization;
                                    organizationStringToObject[rowOrgName] = newString;
                                }
                                else
                                {
                                    organizationStringToObject[rowOrgName] = organizationName;
                                }

                                string text = reader.GetString(3).Replace("\n", " ");
                                QuestionExecution questionExecution = questionTextToObject.ContainsKey(text) ? questionTextToObject[text] : _context.QuestionExecutions.Where(qe => qe.Body == text && qe.ExecutionId == execution.Id).FirstOrDefault();

                                if (questionExecution == null)
                                {
                                    Question newQuestion = new Question
                                    {
                                        QuestionType = currentType
                                    };
                                    QuestionExecution newQuestionExecution = new QuestionExecution
                                    {
                                        Position = Int32.Parse(reader.GetString(2).Replace("Q", "")),
                                        Body = text,
                                        Execution = execution,
                                        Question = newQuestion
                                    };

                                    _context.Questions.Add(newQuestion);
                                    _context.QuestionExecutions.Add(newQuestionExecution);

                                    questionExecution = newQuestionExecution;
                                    questionTextToObject[text] = newQuestionExecution;
                                }
                                else
                                {
                                    questionTextToObject[text] = questionExecution;
                                }

                                int respondents = reader.GetFieldType(4) == "".GetType() ? Int32.Parse(reader.GetString(4).Replace(",", "")) : (int)(reader.GetDouble(4));

                                for (int i = POSSIBLE_RESPONSE_START; i < reader.FieldCount - 1; i++)
                                {
                                    decimal count = reader.GetFieldType(i) == "".GetType() ? 0 : (decimal)(respondents * reader.GetDouble(i));

                                    _context.Responses.Add(new Models.Response
                                    {
                                        Count = count,
                                        QuestionExecution = questionExecution,
                                        PossibleResponse = colToPossibleResponse[i],
                                        DataGroup = organization
                                    });
                                }
                            }
                        }
                        // In final version, need to create a new question type when this happens.
                        // But this is fine for the Core Survey-only start.
                    } while (reader.NextResult());
                }
            }

            // Might have to move up, but for now I want to see if associations work in this setup.
            _context.SaveChanges();

            return Ok();
        }

        // Breaking C# capitalized class variable names because I want my API to use lower-case querying params etc.
        public class UploadModel
        {
            public string key { get; set; }
            public string notes { get; set; }
            public IFormFile file { get; set; }
        }
    }
}
