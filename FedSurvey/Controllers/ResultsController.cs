using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FedSurvey.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace FedSurvey.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly CoreDbContext _context;
        private readonly IConfiguration _configuration;

        public ResultsController(CoreDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // This is so ugly!
        // Not sure if it is just static typing in a method like this
        // or if I did some things wrong.
        // Consider making all filters ID-based?
        [HttpGet]
        public IEnumerable<ResultDTO> Get(
            [FromQuery(Name = "question-ids")] List<int> questionIds,
            [FromQuery(Name = "data-group-names")] List<string> dataGroupNames,
            [FromQuery(Name = "possible-response-names")] List<string> possibleResponseNames,
            [FromQuery(Name = "execution-keys")] List<string> executionKeys
        ) {
            // {0} is BottomLevel query string,
            // {1} is MiddleLevel query string
            string queryFormatString = @"WITH BottomLevel AS (
                SELECT
                    Executions.""Key"" AS ExecutionName,
                    Executions.OccurredTime AS ExecutionTime,
                    PossibleResponseStrings.Name AS PossibleResponseName,
                    PossibleResponses.PartOfPercentage AS PartOfPercentage,
                    Responses.Count,
                    CASE WHEN PossibleResponses.PartOfPercentage = 0 THEN NULL ELSE Responses.Count / (CASE WHEN SUM(QuestionExecutionResponses.Count) = 0 THEN 1 ELSE SUM(QuestionExecutionResponses.Count) END) * 100 END AS Percentage,
                    DataGroups.Id AS DataGroupId,
                    DataGroupStrings.Name AS DataGroupName,
                    QuestionExecutions.QuestionId AS QuestionId,
                    QuestionExecutions.Position AS QuestionNumber
                FROM (
                    SELECT
                        SUM(Responses.Count) AS Count,
                        Responses.QuestionExecutionId,
                        Responses.PossibleResponseId,
                        Responses.DataGroupId
                    FROM Responses
                    GROUP BY
                    Responses.QuestionExecutionId,
                    Responses.PossibleResponseId,
                    Responses.DataGroupId
                ) Responses
                JOIN PossibleResponses
                ON PossibleResponses.Id = Responses.PossibleResponseId
                JOIN PossibleResponseStrings
                ON PossibleResponseStrings.PossibleResponseId = PossibleResponses.Id
                    AND PossibleResponseStrings.Preferred = 1
                JOIN QuestionExecutions
                ON QuestionExecutions.Id = Responses.QuestionExecutionId
                JOIN Executions
                ON Executions.Id = QuestionExecutions.ExecutionId
                JOIN DataGroups
                ON DataGroups.Id = Responses.DataGroupId
                JOIN DataGroupStrings
                ON DataGroupStrings.DataGroupId = DataGroups.Id
                    AND DataGroupStrings.Preferred = 1
                LEFT JOIN (
                    SELECT Responses.*
                    FROM Responses
                    JOIN PossibleResponses
                    ON PossibleResponses.Id = Responses.PossibleResponseId
                    WHERE PossibleResponses.PartOfPercentage = 1
                ) QuestionExecutionResponses
                    ON QuestionExecutionResponses.QuestionExecutionId = Responses.QuestionExecutionId
                        AND QuestionExecutionResponses.DataGroupId = Responses.DataGroupId
                {0}
                GROUP BY
                Executions.""Key"",
                Executions.OccurredTime,
                PossibleResponseStrings.Name,
                Responses.Count,
                PossibleResponses.PartOfPercentage,
                DataGroups.Id,
                DataGroupStrings.Name,
                QuestionExecutions.QuestionId,
                QuestionExecutions.Position
            ),
            MiddleLevel AS (
                SELECT
                    BottomLevel.ExecutionName,
                    BottomLevel.ExecutionTime,
                    BottomLevel.PossibleResponseName,
                    BottomLevel.PartOfPercentage,
                    SUM(BottomLevel.Count) AS Count,
                    DataGroups.Id AS DataGroupId,
                    BottomLevel.QuestionId,
                    BottomLevel.QuestionNumber
                FROM DataGroups
                JOIN DataGroupLinks
                ON DataGroupLinks.ParentId = DataGroups.Id
                JOIN BottomLevel
                ON BottomLevel.DataGroupId = DataGroupLinks.ChildId
                JOIN DataGroupStrings
                ON DataGroupStrings.DataGroupId = DataGroups.Id
                    AND DataGroupStrings.Preferred = 1
                {1}
                GROUP BY
                BottomLevel.ExecutionName,
                BottomLevel.ExecutionTime,
                BottomLevel.PossibleResponseName,
                BottomLevel.PartOfPercentage,
                DataGroups.Id,
                BottomLevel.QuestionId,
                BottomLevel.QuestionNumber
            ),
            ComputedTotals AS (
                SELECT
                    MiddleLevel.QuestionId,
                    MiddleLevel.ExecutionName,
                    SUM(MiddleLevel.Count) AS Count
                FROM MiddleLevel
                WHERE MiddleLevel.PartOfPercentage = 1
                GROUP BY
                MiddleLevel.QuestionId,
                MiddleLevel.ExecutionName
            ),
            -- Possible it would be better to have a different solution that better takes the latest
            -- question text.
            QuestionTexts AS (
                SELECT
                    MAX(QuestionExecutions.Body) AS Body,
                    QuestionExecutions.QuestionId
                FROM QuestionExecutions
                GROUP BY QuestionExecutions.QuestionId
            )

            SELECT
                BottomLevel.ExecutionName,
                BottomLevel.ExecutionTime,
                BottomLevel.PossibleResponseName,
                BottomLevel.Count,
                BottomLevel.Percentage,
                QuestionTexts.Body,
                BottomLevel.DataGroupName,
                BottomLevel.QuestionId,
                BottomLevel.QuestionNumber
            FROM BottomLevel
            JOIN QuestionTexts
            ON QuestionTexts.QuestionId = BottomLevel.QuestionId
            {2}

            UNION

            SELECT
                MiddleLevel.ExecutionName,
                MiddleLevel.ExecutionTime,
                MiddleLevel.PossibleResponseName,
                MiddleLevel.Count,
                CASE WHEN MiddleLevel.PartOfPercentage = 0 THEN NULL ELSE MiddleLevel.Count / (CASE WHEN ComputedTotals.Count = 0 THEN 1 ELSE ComputedTotals.Count END) * 100 END AS Percentage,
                QuestionTexts.Body,
                DataGroupStrings.Name AS DataGroupName,
                MiddleLevel.QuestionId,
                MiddleLevel.QuestionNumber
            FROM MiddleLevel
            JOIN ComputedTotals
            ON ComputedTotals.QuestionId = MiddleLevel.QuestionId
            AND ComputedTotals.ExecutionName = MiddleLevel.ExecutionName
            JOIN DataGroupStrings
            ON DataGroupStrings.DataGroupId = MiddleLevel.DataGroupId
            AND DataGroupStrings.Preferred = 1
            JOIN QuestionTexts
            ON QuestionTexts.QuestionId = MiddleLevel.QuestionId
            {3}";

            string connectionString = _configuration.GetConnectionString("Database");

            List<ResultDTO> results = new List<ResultDTO>();

            // Need to add in children data group names.
            // This can be optimized to one SQL query, but SQL in C# has proven to be a major pain.
            List<int> dgIds = _context.DataGroupStrings.Where(dgs => dataGroupNames.Contains(dgs.Name)).Select(dgs => dgs.DataGroupId).ToList();
            List<int> childIds = _context.DataGroupLinks.Where(dgl => dgIds.Contains(dgl.ParentId)).Select(dgl => dgl.ChildId).ToList();
            List<string> childNames = _context.DataGroupStrings.Where(dgs => childIds.Contains(dgs.DataGroupId) && dgs.Preferred).Select(dgs => dgs.Name).ToList();

            // Need this for the sublist queries then need to query the overall queries back to normal.
            List<string> dataGroupNamesWithChildren = dataGroupNames.Concat(childNames).ToList();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    string[] questionIdParameters = new string[questionIds.Count];
                    string[] dataGroupNameParameters = new string[dataGroupNamesWithChildren.Count];
                    string[] possibleResponseNameParameters = new string[possibleResponseNames.Count];
                    string[] executionKeyParameters = new string[executionKeys.Count];

                    for (int i = 0; i < questionIds.Count; i++)
                    {
                        questionIdParameters[i] = string.Format("@QuestionIds{0}", i);
                        command.Parameters.AddWithValue(questionIdParameters[i], questionIds[i]);
                    }

                    for (int i = 0; i < dataGroupNamesWithChildren.Count; i++)
                    {
                        dataGroupNameParameters[i] = string.Format("@DataGroupNames{0}", i);
                        command.Parameters.AddWithValue(dataGroupNameParameters[i], dataGroupNamesWithChildren[i]);
                    }

                    for (int i = 0; i < possibleResponseNames.Count; i++)
                    {
                        possibleResponseNameParameters[i] = string.Format("@PossibleResponseNames{0}", i);
                        command.Parameters.AddWithValue(possibleResponseNameParameters[i], possibleResponseNames[i]);
                    }

                    for (int i = 0; i < executionKeys.Count; i++)
                    {
                        executionKeyParameters[i] = string.Format("@ExecutionKeys{0}", i);
                        command.Parameters.AddWithValue(executionKeyParameters[i], executionKeys[i]);
                    }

                    // Horribly needs to be patternized.
                    string firstQuestionIdsQuery = questionIds.Count > 0 ? string.Format("QuestionExecutions.QuestionId IN ({0})", string.Join(", ", questionIdParameters)) : null;
                    string secondQuestionIdsQuery = questionIds.Count > 0 ? string.Format("BottomLevel.QuestionId IN ({0})", string.Join(", ", questionIdParameters)) : null;

                    string firstDataGroupNamesQuery = dataGroupNamesWithChildren.Count > 0 ? string.Format("DataGroupStrings.Name IN ({0})", string.Join(", ", dataGroupNameParameters)) : null;
                    string secondDataGroupNamesQuery = firstDataGroupNamesQuery;

                    string firstExecutionKeysQuery = executionKeys.Count > 0 ? string.Format("Executions.\"Key\" IN ({0})", string.Join(", ", executionKeys)) : null;
                    string secondExecutionKeysQuery = executionKeys.Count > 0 ? string.Format("BottomLevel.ExecutionName IN ({0})", string.Join(", ", executionKeys)) : null;

                    string firstQuery = "";
                    string secondQuery = "";

                    if (questionIds.Count > 0 || dataGroupNamesWithChildren.Count > 0 || possibleResponseNames.Count > 0 || executionKeys.Count > 0)
                    {
                        firstQuery = "WHERE ";
                        secondQuery = "WHERE ";

                        List<string> firstQueries = new List<string>();
                        List<string> secondQueries = new List<string>();

                        if (firstQuestionIdsQuery != null)
                        {
                            firstQueries.Add(firstQuestionIdsQuery);
                        }

                        if (firstDataGroupNamesQuery != null)
                        {
                            firstQueries.Add(firstDataGroupNamesQuery);
                        }

                        if (firstExecutionKeysQuery != null)
                        {
                            firstQueries.Add(firstExecutionKeysQuery);
                        }

                        if (secondQuestionIdsQuery != null)
                        {
                            secondQueries.Add(secondQuestionIdsQuery);
                        }

                        if (secondDataGroupNamesQuery != null)
                        {
                            secondQueries.Add(secondDataGroupNamesQuery);
                        }

                        if (secondExecutionKeysQuery != null)
                        {
                            secondQueries.Add(secondExecutionKeysQuery);
                        }

                        firstQuery += string.Join(" AND ", firstQueries);
                        secondQuery += string.Join(" AND ", secondQueries);
                    }

                    string[] specificDataGroupNameParameters = new string[dataGroupNames.Count];

                    for (int i = 0; i < dataGroupNames.Count; i++)
                    {
                        specificDataGroupNameParameters[i] = string.Format("@DataGroupSpecificNames{0}", i);
                        command.Parameters.AddWithValue(specificDataGroupNameParameters[i], dataGroupNames[i]);
                    }

                    string thirdQuery = "";
                    string fourthQuery = "";

                    string thirdDataGroupsQuery = dataGroupNames.Count > 0 ? string.Format("DataGroupName IN ({0})", string.Join(", ", specificDataGroupNameParameters)) : null;
                    string fourthDataGroupsQuery = dataGroupNames.Count > 0 ? string.Format("DataGroupStrings.Name IN ({0})", string.Join(", ", specificDataGroupNameParameters)) : null;

                    string thirdPossibleResponseNamesQuery = possibleResponseNames.Count > 0 ? string.Format("PossibleResponseName IN ({0})", string.Join(", ", possibleResponseNameParameters)) : null;
                    string fourthPossibleResponseNamesQuery = possibleResponseNames.Count > 0 ? string.Format("MiddleLevel.PossibleResponseName IN ({0})", string.Join(", ", possibleResponseNameParameters)) : null;

                    if (dataGroupNames.Count > 0 || possibleResponseNames.Count > 0)
                    {
                        thirdQuery = "WHERE ";
                        fourthQuery = "WHERE ";

                        List<string> thirdQueries = new List<string>();
                        List<string> fourthQueries = new List<string>();

                        if (thirdDataGroupsQuery != null)
                        {
                            thirdQueries.Add(thirdDataGroupsQuery);
                        }

                        if (thirdPossibleResponseNamesQuery != null)
                        {
                            thirdQueries.Add(thirdPossibleResponseNamesQuery);
                        }

                        if (fourthDataGroupsQuery != null)
                        {
                            fourthQueries.Add(fourthDataGroupsQuery);
                        }

                        if (fourthPossibleResponseNamesQuery != null)
                        {
                            fourthQueries.Add(fourthPossibleResponseNamesQuery);
                        }

                        thirdQuery += string.Join(" AND ", thirdQueries);
                        fourthQuery += string.Join(" AND ", fourthQueries);
                    }

                    command.CommandText = string.Format(queryFormatString, firstQuery, secondQuery, thirdQuery, fourthQuery);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new ResultDTO
                            {
                                ExecutionName = reader.GetString(0),
                                ExecutionTime = reader.GetDateTime(1),
                                PossibleResponseName = reader.GetString(2),
                                Count = reader.GetDecimal(3),
                                Percentage = reader.IsDBNull(4) ? null : (decimal?) reader.GetDecimal(4),
                                QuestionText = reader.GetString(5),
                                DataGroupName = reader.GetString(6),
                                QuestionId = reader.GetInt32(7),
                                QuestionNumber = reader.GetInt32(8)
                            });
                        }
                    }
                }
            }

            return results;
        }
    }
}
