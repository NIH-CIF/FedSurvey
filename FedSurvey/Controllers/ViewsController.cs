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
    public class ViewsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public ViewsController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        [Route("api/[controller]")]
        public IEnumerable<View.DTO> Get()
        {
            IEnumerable<View> views = _context.Views.Include(x => x.ViewConfigs);

            return views.Select(x => View.ToDTO(x)).ToList();
        }
    }
}
