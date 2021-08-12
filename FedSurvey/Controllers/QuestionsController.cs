using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FedSurvey.Models;
using FedSurvey.Services;
using Microsoft.EntityFrameworkCore;

namespace FedSurvey.Controllers
{
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly CoreDbContext _context;
        private readonly TokenService _tokenService;

        public QuestionsController(CoreDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
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

        [HttpPost]
        [Route("api/[controller]/merge")]
        public IActionResult Merge([FromBody] List<string> texts)
        {
            if (!_tokenService.IsValidHeaders(Request.Headers, _context))
            {
                return Unauthorized();
            }

            // handle ids being empty

            List<int> questionIds = _context.QuestionExecutions.Where(qe => texts.Contains(qe.Body)).Select(qe => qe.QuestionId).Distinct().ToList();

            int unifiedQuestion = questionIds.First();
            List<int> others = questionIds.Skip(1).ToList();

            // need to verify questions are the same question type
            IQueryable<QuestionExecution> otherQuestionExecutions = _context.QuestionExecutions.Where(qe => others.Contains(qe.QuestionId));
            foreach (QuestionExecution qe in otherQuestionExecutions)
            {
                qe.QuestionId = unifiedQuestion;
            }

            List<int> mainQuestionGroups = _context.QuestionLinks.Where(ql => unifiedQuestion == ql.QuestionId).Select(x => x.QuestionGroupId).ToList();
            IQueryable<QuestionLink> otherQuestionLinks = _context.QuestionLinks.Where(ql => others.Contains(ql.QuestionId));
            foreach (QuestionLink ql in otherQuestionLinks)
            {
                if (mainQuestionGroups.Contains(ql.QuestionGroupId))
                {
                    _context.QuestionLinks.Remove(ql);
                }
                else
                {
                    ql.QuestionId = unifiedQuestion;
                    mainQuestionGroups.Add(ql.QuestionGroupId);
                }
            }

            IQueryable<Question> questionsToDelete = _context.Questions.Where(q => others.Contains(q.Id));
            foreach (Question q in questionsToDelete)
            {
                _context.Questions.Remove(q);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
