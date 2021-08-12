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
    public class DataGroupsController : ControllerBase
    {
        private readonly CoreDbContext _context;
        private readonly TokenService _tokenService;

        public DataGroupsController(CoreDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        [Route("api/data-groups")]
        public IActionResult Get(
            [FromQuery] bool computed
        ) {
            if (!_tokenService.IsValidHeaders(Request.Headers, _context))
            {
                return Unauthorized();
            }

            IEnumerable<DataGroup> dataGroups = _context.DataGroups.Include(x => x.DataGroupStrings).Include(x => x.ParentLinks);

            if (computed)
            {
                dataGroups = dataGroups.Where(x => x.ParentLinks.Count > 0);
            }
            // Note that this will break if multiple filters are enacted.
            else if (!string.IsNullOrEmpty(Request.QueryString.Value))
            {
                dataGroups = dataGroups.Where(x => x.ParentLinks.Count == 0);
            }

            return new JsonResult(dataGroups.Select(x => DataGroup.ToDTO(x)).ToList().OrderBy(x => x.Name));
        }

        // Accepts list of ids like "[2,6]" as JSON body and merges them.
        // Proposal for new functionality:
        // If a name of a data group is optionally provided, the merge will occur into a newly created data group.
        // Otherwise, the later data groups will be deleted and only the first data group will be left.
        // Make a computed group so that we do not copy data.
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
                dgs.Preferred = false;
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

        [HttpPost]
        [Route("api/data-groups/create")]
        public IActionResult Create([FromBody] CreateModel createModel)
        {
            if (createModel.name == null)
            {
                return UnprocessableEntity();
            }

            // future: ensure name is not already taken
            DataGroup newOrganization = new DataGroup { };
            DataGroupString newOrgString = new DataGroupString
            {
                DataGroup = newOrganization,
                Name = createModel.name,
                Preferred = true
            };

            _context.DataGroups.Add(newOrganization);
            _context.DataGroupStrings.Add(newOrgString);

            // future: ensure no children are parents
            if (createModel.linkIds.Count > 0)
            {
                foreach (int id in createModel.linkIds)
                {
                    // future: ensure id is valid
                    DataGroupLink newLink = new DataGroupLink
                    {
                        Parent = newOrganization,
                        ChildId = id
                    };

                    _context.Add(newLink);
                }
            }

            _context.SaveChanges();

            return Ok();
        }

        // Breaking C# capitalized class variable names because I want my API to use lower-case querying params etc.
        public class CreateModel
        {
            public string name { get; set; }
            public List<int> linkIds { get; set; }
        }
    }
}
