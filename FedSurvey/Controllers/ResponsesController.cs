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
        // Later, this should get cleaned up to not compute Percentage any more.
        [HttpGet]
        public IEnumerable<ResponseDTO> Get(
            [FromQuery(Name = "question-execution-ids")] List<int> questionExecutionIds,
            [FromQuery(Name = "data-group-ids")] List<int> dataGroupIds
        ) {
            IEnumerable<ResponseDTO> responses = _context.ResponseDTOs;

            // This would be optimized by including in the manual SQL, but save that for after benchmarking.
            if (questionExecutionIds.Count > 0)
            {
                responses = responses.Where(x => questionExecutionIds.Contains(x.QuestionExecutionId));
            }

            if (dataGroupIds.Count > 0)
            {
                responses = responses.Where(x => dataGroupIds.Contains(x.DataGroupId));
            }

            return responses.ToList();
        }
    }
}
