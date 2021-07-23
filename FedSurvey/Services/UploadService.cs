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
using Microsoft.VisualBasic.FileIO;

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

                            if (execution == null)
                            {
                                execution = FindOrCreateExecution(context, key, notes);
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
                                        Name = rowOrgName,
                                        Preferred = true
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

        public static bool UploadSurveyMonkeyFormat(CoreDbContext context, string key, string notes, string dataGroupName, IFormFile file)
        {
            using (var parser = new TextFieldParser(file.OpenReadStream()))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // Move past the title line.
                parser.ReadLine();

                List<string[]> questionLines = new List<string[]>();
                List<List<string[]>> questionBlocks = new List<List<string[]>>();

                while (!parser.EndOfData)
                {
                    string[] next = parser.ReadFields();

                    // Blank line means that we should keep looking for the start of new data.
                    if (next.Length == 1)
                    {
                        if (questionLines.Count > 0)
                        {
                            questionBlocks.Add(questionLines);
                            questionLines = new List<string[]>();
                        }

                        questionLines.Add(next);
                    }
                    else
                    {
                        questionLines.Add(next);
                    }
                }

                if (questionLines.Count > 0)
                {
                    questionBlocks.Add(questionLines);
                    questionLines = new List<string[]>();
                }

                Execution execution = null;
                DataGroup dataGroup = null;

                // Hardcoded for now.
                QuestionTypeString savedString = context.QuestionTypeStrings.Where(qts => qts.Name.Equals("Core Survey")).Include(x => x.QuestionType).FirstOrDefault();
                QuestionType currentType = savedString != null ? savedString.QuestionType : null;

                Dictionary<string, PossibleResponse> textToPossibleResponse = new Dictionary<string, PossibleResponse>();

                foreach (List<string[]> block in questionBlocks)
                {
                    string titleLine = block[0][0];
                    string regex = @"^Q(\d+)\. ";
                    Match match = Regex.Match(titleLine, regex);

                    int position = Int32.Parse(match.Groups[1].Captures[0].Value);
                    string title = Regex.Replace(titleLine, regex, "");

                    Dictionary<string, int> possibleResponseStringToCount = new Dictionary<string, int>();
                    
                    foreach (string[] line in block.Skip(1))
                    {
                        // Header row
                        if (line[0].Equals("Answer Choices"))
                        {
                            // Do not bother with the alternate table format for now.
                            if (line[1].Equals("Response Percent") && line[2].Equals("Responses"))
                                continue;
                            else
                                break;
                        }

                        string firstNonEmpty = line.First(label => !label.Equals(""));

                        // Answered total count
                        if (firstNonEmpty.Equals("Answered"))
                            continue;

                        // somewhat a fallacy to call this "second"
                        int secondNonEmpty = Int32.Parse(line.Last(label => !label.Equals("")));
                        possibleResponseStringToCount[line[0].Equals("") ? firstNonEmpty : line[0]] = secondNonEmpty;
                    }

                    if (possibleResponseStringToCount.Count == 0)
                        continue;

                    // Check our possible response options and make sure they belong to the right question type.
                    List<string> possibleResponseStrings = new List<string>(possibleResponseStringToCount.Keys);
                    bool validQuestionTypes = true;

                    foreach (string possibleResponseString in possibleResponseStrings)
                    {
                        if (textToPossibleResponse.ContainsKey(possibleResponseString))
                            continue;

                        PossibleResponseString responseName = context.PossibleResponseStrings.Where(prs => prs.Name.Equals(possibleResponseString)).Include(x => x.PossibleResponse).FirstOrDefault();

                        if (responseName == null || responseName.PossibleResponse.QuestionTypeId != currentType.Id)
                        {
                            System.Diagnostics.Debug.WriteLine(possibleResponseString + " belonging to other question type");
                            validQuestionTypes = false;
                        }
                        else
                        {
                            textToPossibleResponse[possibleResponseString] = responseName.PossibleResponse;
                        }
                    }

                    if (!validQuestionTypes)
                        continue;

                    // title is text to compare question upon
                    // position is the question position in this case
                    // data group needs to be supplied from outside
                    // execution needs to be applied from outside
                    // possible response will come from the dictionary
                    if (execution == null)
                    {
                        execution = FindOrCreateExecution(context, key, notes);
                    }

                    QuestionExecution questionExecution = context.QuestionExecutions.Where(qe => qe.Body.Equals(title) && qe.ExecutionId == execution.Id).FirstOrDefault();

                    if (questionExecution == null)
                    {
                        Question newQuestion = new Question
                        {
                            QuestionType = currentType
                        };
                        QuestionExecution newQuestionExecution = new QuestionExecution
                        {
                            Position = position,
                            Body = title,
                            Execution = execution,
                            Question = newQuestion
                        };

                        context.Questions.Add(newQuestion);
                        context.QuestionExecutions.Add(newQuestionExecution);

                        questionExecution = newQuestionExecution;
                    }

                    // Make new organization if one does not already exist.
                    if (dataGroup == null)
                    {
                        DataGroupString organizationName = context.DataGroupStrings.Where(dgs => dgs.Name.Equals(dataGroupName)).Include(x => x.DataGroup).FirstOrDefault();
                        dataGroup = organizationName != null ? organizationName.DataGroup : null;

                        if (dataGroup == null)
                        {
                            DataGroup newOrganization = new DataGroup { };
                            DataGroupString newString = new DataGroupString
                            {
                                DataGroup = newOrganization,
                                Name = dataGroupName,
                                Preferred = true
                            };

                            context.DataGroups.Add(newOrganization);
                            context.DataGroupStrings.Add(newString);

                            dataGroup = newOrganization;
                        }
                    }

                    foreach (KeyValuePair<string, int> possibleResponseResults in possibleResponseStringToCount)
                    {
                        context.Responses.Add(new Models.Response
                        {
                            Count = possibleResponseResults.Value,
                            QuestionExecution = questionExecution,
                            PossibleResponse = textToPossibleResponse[possibleResponseResults.Key],
                            DataGroup = dataGroup
                        });
                    }
                }
            }

            context.SaveChanges();

            return true;
        }

        private static Execution FindOrCreateExecution(CoreDbContext context, string key, string notes)
        {
            Execution execution = context.Executions.Where(e => e.Key.Equals(key)).FirstOrDefault();

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
                context.SaveChanges();
            }

            return execution;
        }
    }
}
