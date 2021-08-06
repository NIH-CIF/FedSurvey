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
    public class QuestionsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public QuestionsController(CoreDbContext context)
        {
            _context = context;
        }

        // One guide recommended using async more often here.
        [HttpGet]
        [Route("api/[controller]")]
        public IEnumerable<QuestionDTO> Get() {
            IEnumerable<QuestionDTO> questions = _context.QuestionDTOs;

            return questions.ToList();
        }

        // Use QuestionDTO for this too, then just have a boolean change the query?
        // Would be better API design.
        [HttpGet]
        [Route("api/[controller]/merge-candidates")]
        public IEnumerable<MergeCandidateDTO> GetMergeCandidates()
        {
            IEnumerable<MergeCandidateDTO> mergeCandidates = _context.MergeCandidateDTOs;

            return mergeCandidates.ToList();
        }
    }
}
