using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;
using Microsoft.EntityFrameworkCore;

namespace FedSurvey.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsesController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public ResponsesController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        public IEnumerable<Response.DTO> Get(
            [FromQuery(Name = "question-execution-ids")] List<int> questionExecutionIds,
            [FromQuery(Name = "data-group-ids")] List<int> dataGroupIds
        ) {
            // do FromSqlRaw, try using Percentage as field first
            IEnumerable<Response> responses = _context.Responses.FromSqlRaw(
                @"SELECT
                  Responses.Id,
                  Responses.Count,
                  Responses.QuestionExecutionId,
                  Responses.PossibleResponseId,
                  Responses.DataGroupId,
                  SUM(QuestionExecutionResponses.Count) AS Total
                FROM Responses
                LEFT JOIN Responses QuestionExecutionResponses
                ON QuestionExecutionResponses.QuestionExecutionId = Responses.QuestionExecutionId
                GROUP BY
                Responses.Id,
                Responses.Count,
                Responses.QuestionExecutionId,
                Responses.PossibleResponseId,
                Responses.DataGroupId"
            );

            if (questionExecutionIds.Count > 0)
            {
                responses = responses.Where(x => questionExecutionIds.Contains(x.QuestionExecutionId));
            }

            if (dataGroupIds.Count > 0)
            {
                responses = responses.Where(x => dataGroupIds.Contains(x.DataGroupId));
            }

            return responses.Select(x => Models.Response.ToDTO(x)).ToList();
        }
    }
}
