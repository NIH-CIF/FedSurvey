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
        public IActionResult Upload(IFormFile file)
        {
            // Starting by assuming 2016 thru 2019 format.
            // 2020 will have to be programmed for specially anyway.
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        // Later consideration: is doing n queries where n is the number of sheets okay?
                        QuestionType currentType = _context.QuestionTypes.Where(qt => qt.Name == reader.Name).FirstOrDefault();

                        if (currentType != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Processing " + reader.Name);

                            while (reader.Read())
                            {
                                // Sorting level
                            }
                        }
                    } while (reader.NextResult());
                }
            }

            return Ok();
        }
    }
}
