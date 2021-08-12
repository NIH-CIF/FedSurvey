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
        private readonly TokenService _tokenService;

        public UploadController(CoreDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [Route("api/[controller]/format")]
        [HttpPost]
        public IActionResult GetFormat([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return UnprocessableEntity();
            }

            if (UploadService.IsFEVSFormat(file))
            {
                return new JsonResult(new { format = "fevs" });
            }
            else if (UploadService.IsNewFormat(file))
            {
                return new JsonResult(new { format = "new" });
            }
            else if (UploadService.IsSurveyMonkeyFormat(file))
            {
                return new JsonResult(new { format = "survey-monkey" });
            }
            else
            {
                return new JsonResult(new { format = "unknown" });
            }
        }

        [Route("api/[controller]")]
        [HttpPost]
        public IActionResult Upload([FromForm] UploadModel uploadModel)
        {
            if (!_tokenService.IsValidHeaders(Request.Headers, _context))
            {
                return Unauthorized();
            }

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
            else if (UploadService.IsNewFormat(uploadModel.file) && UploadService.UploadNewFormat(_context, uploadModel.key, uploadModel.notes, executionDate, uploadModel.file))
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
