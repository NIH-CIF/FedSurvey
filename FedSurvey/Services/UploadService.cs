using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExcelDataReader;
using FedSurvey.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FedSurvey.Services
{
    public static class UploadService
    {
        public static bool IsFEVSFormat(IFormFile file)
        {
            string extension = System.IO.Path.GetExtension(file.FileName);

            if (!extension.Equals(".xlsx"))
            {
                return false;
            }

            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        // This approach is because the 2018 raw data has a header row that comes later than the first line
                        // on one of the sheets.
                        bool headerRowExists = false;

                        while (reader.Read())
                        {
                            // Maybe more validation to do, but the upload should be able to handle bad data.
                            if (reader.GetString(0).Replace("\n", " ").Equals("Sorting Level") &&
                                reader.GetString(1).Replace("\n", " ").Equals("Organization") &&
                                reader.GetString(2).Replace("\n", " ").Equals("Item") &&
                                reader.GetString(3).Replace("\n", " ").Equals("Item Text") &&
                                reader.GetString(4).Replace("\n", " ").Equals("Item Respondents N"))
                                return headerRowExists = true;
                        }

                        if (!headerRowExists)
                            return false;
                    } while (reader.NextResult());
                }
            }

            return true;
        }

        public static bool UploadFEVSFormat(CoreDbContext context, string key, string notes, IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // We will store the execution we are attaching to far outside so
                    // that we only load it once.
                    Execution execution = null;

                    do
                    {
                        // Later consideration: is doing n queries where n is the number of sheets okay?
                        QuestionTypeString savedString = context.QuestionTypeStrings.Where(qts => qts.Name.Equals(reader.Name)).Include(x => x.QuestionType).FirstOrDefault();
                        QuestionType currentType = savedString != null ? savedString.QuestionType : null;

                        if (currentType != null)
                        {
                            // Pull out a PossibleResponse mapping from the header row.
                            reader.Read();

                            const int POSSIBLE_RESPONSE_START = 5;
                            Dictionary<int, PossibleResponse> colToPossibleResponse = new Dictionary<int, PossibleResponse>();

                            // Stores whether the specified column is an exact number or a percentage.
                            Dictionary<int, bool> isPercent = new Dictionary<int, bool>();

                            for (int i = POSSIBLE_RESPONSE_START; i < reader.FieldCount; i++)
                            {
                                string responseWithSuffix = reader.GetString(i).Replace("\n", " ");
                                isPercent[i] = responseWithSuffix.EndsWith('%');

                                string response = Regex.Replace(responseWithSuffix, @"[%|N]$", "").Trim();

                                PossibleResponseString responseName = context.PossibleResponseStrings.Where(prs => prs.Name.Equals(response)).Include(x => x.PossibleResponse).FirstOrDefault();

                                if (responseName == null)
                                {
                                    System.Diagnostics.Debug.WriteLine(response + " causing trouble");
                                    return false;
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
                                execution = context.Executions.Where(e => e.Key.Equals(key)).FirstOrDefault();

                                if (execution == null)
                                {
                                    Execution newExecution = new Execution
                                    {
                                        Key = key,
                                        Notes = notes
                                    };
                                    context.Executions.Add(newExecution);
                                    execution = newExecution;

                                    // Save changes so that execution has an ID for comparison later.
                                    // Can this be refactored out?
                                    context.SaveChanges();
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
                                DataGroupString organizationName = organizationStringToObject.ContainsKey(rowOrgName) ? organizationStringToObject[rowOrgName] : context.DataGroupStrings.Where(dgs => dgs.Name.Equals(rowOrgName)).Include(x => x.DataGroup).FirstOrDefault();
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

                                    context.DataGroups.Add(newOrganization);
                                    context.DataGroupStrings.Add(newString);

                                    organization = newOrganization;
                                    organizationStringToObject[rowOrgName] = newString;
                                }
                                else
                                {
                                    organizationStringToObject[rowOrgName] = organizationName;
                                }

                                string text = reader.GetString(3).Replace("\n", " ");
                                QuestionExecution questionExecution = questionTextToObject.ContainsKey(text) ? questionTextToObject[text] : context.QuestionExecutions.Where(qe => qe.Body.Equals(text) && qe.ExecutionId == execution.Id).FirstOrDefault();

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

                                    context.Questions.Add(newQuestion);
                                    context.QuestionExecutions.Add(newQuestionExecution);

                                    questionExecution = newQuestionExecution;
                                    questionTextToObject[text] = newQuestionExecution;
                                }
                                else
                                {
                                    questionTextToObject[text] = questionExecution;
                                }

                                int respondents = reader.GetFieldType(4) == "".GetType() ? Int32.Parse(reader.GetString(4).Replace(",", "")) : (int)(reader.GetDouble(4));

                                for (int i = POSSIBLE_RESPONSE_START; i < reader.FieldCount; i++)
                                {
                                    decimal count = reader.GetFieldType(i) == "".GetType() ? 0 : (decimal)((isPercent[i] ? (double) respondents : 1) * reader.GetDouble(i));

                                    context.Responses.Add(new Models.Response
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
            context.SaveChanges();

            return true;
        }

        public static bool IsSurveyMonkeyFormat(IFormFile file)
        {
            string extension = System.IO.Path.GetExtension(file.FileName);

            if (!extension.Equals(".csv"))
            {
                return false;
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                if (reader.ReadLine() == null)
                    return false;

                if (!reader.ReadLine().Equals(""))
                    return false;

                if (!reader.ReadLine().StartsWith('Q'))
                    return false;
            }

            return true;
        }
    }
}
