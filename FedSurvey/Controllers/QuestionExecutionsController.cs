using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;

namespace FedSurvey.Controllers
{
    [ApiController]
    [Route("api/question-executions")]
    public class QuestionExecutionsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public QuestionExecutionsController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        public IEnumerable<QuestionExecution.DTO> Get(
            [FromQuery(Name = "execution-ids")] List<int> executionIds,
            [FromQuery(Name = "question-ids")] List<int> questionIds
        ) {
            IEnumerable<QuestionExecution> questionExecutions = _context.QuestionExecutions.OrderBy(qe => qe.Position);

            if (executionIds.Count > 0)
            {
                questionExecutions = questionExecutions.Where(x => executionIds.Contains(x.ExecutionId));
            }

            if (questionIds.Count > 0)
            {
                questionExecutions = questionExecutions.Where(x => questionIds.Contains(x.QuestionId));
            }

            return questionExecutions.Select(x => QuestionExecution.ToDTO(x)).ToList();
        }
    }
}
