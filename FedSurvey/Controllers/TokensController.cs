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
    public class TokensController : ControllerBase
    {
        private readonly CoreDbContext _context;
        private readonly AuthenticationService _authenticationService;

        public TokensController(CoreDbContext context, AuthenticationService authenticationService)
        {
            _context = context;
            _authenticationService = authenticationService;
        }

        [Route("api/[controller]")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (_authenticationService.Authenticate(login.username, login.password))
            {
                Token token = new Token
                {
                    Body = TokenService.GetUniqueKey(80),
                    ExpiresAt = DateTime.Today.AddDays(7)
                };

                _context.Add(token);
                _context.SaveChanges();

                return new JsonResult(token);
            }

            // else
            return Unauthorized();
        }

        // Breaking C# capitalized class variable names because I want my API to use lower-case querying params etc.
        public class LoginModel
        {
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}
