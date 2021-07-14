using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;
using Microsoft.AspNetCore.Http;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Upload([FromForm] UploadModel uploadModel)
        {
            if (uploadModel.file == null || uploadModel.key == null)
            {
                return UnprocessableEntity();
            }

            // Starting by assuming 2016 thru 2019 format.
            // 2020 will have to be programmed for specially anyway.
            using (var stream = uploadModel.file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // We will store the execution we are attaching to far outside so
                    // that we only load it once.
                    Execution execution = null;

                    do
                    {
                        // Later consideration: is doing n queries where n is the number of sheets okay?
                        QuestionTypeString savedString = _context.QuestionTypeStrings.Where(qts => qts.Name == reader.Name).Include(x => x.QuestionType).FirstOrDefault();
                        QuestionType currentType = savedString != null ? savedString.QuestionType : null;

                        if (currentType != null)
                        {
                            // If we do not have an execution, we need to set it or make one.
                            if (execution == null)
                            {
                                execution = _context.Executions.Where(e => e.Key == uploadModel.key).FirstOrDefault();

                                if (execution == null)
                                {
                                    Execution newExecution = new Execution
                                    {
                                        Key = uploadModel.key,
                                        Notes = uploadModel.notes
                                    };
                                    _context.Executions.Add(newExecution);
                                    execution = newExecution;
                                }
                            }

                            System.Diagnostics.Debug.WriteLine("Processing " + reader.Name);

                            while (reader.Read())
                            {
                                // Columns:
                                // 0 Sorting Level - this will be ignored unless it is later needed
                                // 1 Organization - this is the Data Group
                                // 2 Item - this is the question position once you take off the leading Q
                                // 3 Item Text - this is the question text in this year
                                // 4 Item Respondents - this is the number that will be multiplied by the percentage to get Count
                                // 5-n Options - each column becomes a possible response option
                            }
                        }
                        // In final version, need to create a new question type when this happens.
                        // But this is fine for the Core Survey-only start.
                    } while (reader.NextResult());
                }
            }

            // Might have to move up, but for now I want to see if associations work in this setup.
            _context.SaveChanges();

            return Ok();
        }

        // Breaking C# capitalized class variable names because I want my API to use lower-case querying params etc.
        public class UploadModel
        {
            public string key { get; set; }
            public string notes { get; set; }
            public IFormFile file { get; set; }
        }
    }
}
