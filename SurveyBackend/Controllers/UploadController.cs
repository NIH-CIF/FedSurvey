using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Survey.Models;
using Microsoft.AspNetCore.Http;

namespace Survey.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public UploadController(CoreDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Upload(List<IFormFile> files)
        {
            System.Diagnostics.Debug.WriteLine(files.Count);

            return Ok();
        }
    }
}
