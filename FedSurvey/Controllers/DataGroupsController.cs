using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;

namespace FedSurvey.Controllers
{
    [ApiController]
    [Route("api/data-groups")]
    public class DataGroupsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public DataGroupsController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        public IEnumerable<DataGroup.DTO> Get()
        {
            return _context.DataGroups.Select(x => DataGroup.ToDTO(x)).ToList();
        }
    }
}
