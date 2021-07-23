using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;
using Microsoft.AspNetCore.Http;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using FedSurvey.Services;

namespace FedSurvey.Controllers
{
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public UploadController(CoreDbContext context)
        {
            _context = context;
        }

        [Route("api/[controller]")]
        [HttpPost]
        public IActionResult Upload([FromForm] UploadModel uploadModel)
        {
            if (uploadModel.file == null || uploadModel.key == null)
            {
                return UnprocessableEntity();
            }

            DateTime executionDate = DateTime.UtcNow;

            if (uploadModel.date != null)
            {
                executionDate = DateTime.Parse(uploadModel.date, null, System.Globalization.DateTimeStyles.RoundtripKind).ToUniversalTime();
            }

            // Maybe future validation not to send data group name for FEVS format.
            if (UploadService.IsFEVSFormat(uploadModel.file) && UploadService.UploadFEVSFormat(_context, uploadModel.key, uploadModel.notes, executionDate, uploadModel.file))
            {
                return Ok();
            }
            else if (UploadService.IsSurveyMonkeyFormat(uploadModel.file) && UploadService.UploadSurveyMonkeyFormat(_context, uploadModel.key, uploadModel.notes, executionDate, uploadModel.dataGroupName, uploadModel.file))
            {
                return Ok();
            }
            else
            {
                return UnprocessableEntity();
            }
        }

        // Breaking C# capitalized class variable names because I want my API to use lower-case querying params etc.
        public class UploadModel
        {
            public string key { get; set; }
            public string notes { get; set; }
            public string date { get; set; }
            public string dataGroupName { get; set; }
            public IFormFile file { get; set; }
        }
    }
}
