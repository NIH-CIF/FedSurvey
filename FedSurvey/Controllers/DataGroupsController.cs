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
    public class DataGroupsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public DataGroupsController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        [Route("api/data-groups")]
        public IEnumerable<DataGroup.DTO> Get()
        {
            return _context.DataGroups.Include(x => x.DataGroupStrings).Select(x => DataGroup.ToDTO(x)).ToList();
        }

        // Accepts list of ids like "[2,6]" as JSON body and merges them.
        [HttpPost]
        [Route("api/data-groups/merge")]
        public IActionResult Merge([FromBody] List<int> ids)
        {
            // handle ids being empty

            // We will make the first id in the list be the final data group.
            int unifiedDataGroup = ids.First();
            List<int> others = ids.Skip(1).ToList();

            // Update the other groups' strings to point to the new data group.
            IQueryable<DataGroupString> dataGroupStrings = _context.DataGroupStrings.Where(dgs => others.Contains(dgs.DataGroupId));
            foreach (DataGroupString dgs in dataGroupStrings)
            {
                dgs.DataGroupId = unifiedDataGroup;
            }

            // Update the responses of these data groups to point to the new data group.
            IQueryable<Response> responses = _context.Responses.Where(r => others.Contains(r.DataGroupId));
            foreach (Response r in responses)
            {
                r.DataGroupId = unifiedDataGroup;
            }

            // Delete the old data groups.
            IQueryable<DataGroup> dataGroups = _context.DataGroups.Where(dg => others.Contains(dg.Id));
            foreach (DataGroup dg in dataGroups)
            {
                _context.DataGroups.Remove(dg);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
