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
    public class ResultsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public ResultsController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        public IEnumerable<ResultDTO> Get(
            [FromQuery(Name = "question-ids")] List<int> questionIds,
            [FromQuery(Name = "data-group-names")] List<string> dataGroupNames
        ) {
            IEnumerable<ResultDTO> results = _context.ResultDTOs;

            // This would be optimized by including in the manual SQL, but save that for after benchmarking.
            if (questionIds.Count > 0)
            {
                results = results.Where(x => questionIds.Contains(x.QuestionId));
            }

            if (dataGroupNames.Count > 0)
            {
                results = results.Where(x => dataGroupNames.Contains(x.DataGroupName));
            }

            return results.ToList();
        }
    }
}
