using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;
using Microsoft.AspNetCore.Http;
using ExcelDataReader;

namespace FedSurvey.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var file = files.First();

            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            System.Diagnostics.Debug.WriteLine(reader.GetString(0));
                        }
                    } while (reader.NextResult());
                }
            }

            return Ok();
        }
    }
}
