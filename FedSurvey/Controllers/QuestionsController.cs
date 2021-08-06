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
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public QuestionsController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        public IEnumerable<QuestionDTO> Get(
            [FromQuery(Name = "merge-candidate")] bool mergeCandidate
        ) {
            IEnumerable<QuestionDTO> questions = _context.QuestionDTOs;

            //if (mergeCandidate)
            //{
            //    dataGroups = dataGroups.Where(x => x.ParentLinks.Count > 0);
            //}
            // Note that this will break if multiple filters are enacted.
            //else if (!string.IsNullOrEmpty(Request.QueryString.Value))
            //{
            //    dataGroups = dataGroups.Where(x => x.ParentLinks.Count == 0);
            //}

            return questions.ToList();
        }
    }
}
