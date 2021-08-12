using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;
using FedSurvey.Services;
using Microsoft.EntityFrameworkCore;

namespace FedSurvey.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsesController : ControllerBase
    {
        private readonly CoreDbContext _context;
        private readonly TokenService _tokenService;

        public ResponsesController(CoreDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // One guide recommended using async more often here.
        // Later, this should get cleaned up to not compute Percentage any more.
        [HttpGet]
        public IActionResult Get(
            [FromQuery(Name = "question-execution-ids")] List<int> questionExecutionIds,
            [FromQuery(Name = "data-group-ids")] List<int> dataGroupIds
        ) {
            if (!_tokenService.IsValidHeaders(Request.Headers, _context))
            {
                return Unauthorized();
            }

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

            return new JsonResult(responses.ToList());
        }
    }
}
