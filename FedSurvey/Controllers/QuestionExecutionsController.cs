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
            [FromQuery(Name = "execution-id")] int executionId,
            [FromQuery(Name = "question-id")] int questionId
        ) {
            IEnumerable<QuestionExecution> questionExecutions = _context.QuestionExecutions;

            if (executionId > 0)
            {
                questionExecutions = questionExecutions.Where(x => x.ExecutionId == executionId);
            }

            if (questionId > 0)
            {
                questionExecutions = questionExecutions.Where(x => x.QuestionId == questionId);
            }

            return questionExecutions.Select(x => QuestionExecution.ToDTO(x)).ToList();
        }
    }
}
