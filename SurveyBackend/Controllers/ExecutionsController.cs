using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Survey.Models;

namespace Survey.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecutionsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public ExecutionsController(CoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Execution> Get()
        {
            return _context.Executions.ToList();
        }
    }
}
