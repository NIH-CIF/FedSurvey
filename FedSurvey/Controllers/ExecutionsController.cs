using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;

namespace FedSurvey.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExecutionsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public ExecutionsController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        public IEnumerable<Execution.DTO> Get()
        {
            return _context.Executions.Select(x => Execution.ToDTO(x)).ToList();
        }
    }
}
