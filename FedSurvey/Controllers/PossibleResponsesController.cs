using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;

namespace FedSurvey.Controllers
{
    [ApiController]
    [Route("api/possible-responses")]
    public class PossibleResponsesController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public PossibleResponsesController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        public IEnumerable<PossibleResponse.DTO> Get(
            [FromQuery] List<int> ids
        ) {
            IEnumerable<PossibleResponse> possibleResponses = _context.PossibleResponses;

            if (ids.Count > 0)
            {
                possibleResponses = possibleResponses.Where(x => ids.Contains(x.Id));
            }

            return possibleResponses.Select(x => PossibleResponse.ToDTO(x)).ToList();
        }
    }
}
