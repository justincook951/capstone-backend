using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneQuizAPI.Models;
using CapstoneQuizAPI.DTOs;

namespace CapstoneQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionQuestionsController : ControllerBase
    {
        private readonly CapstoneQuizContext _context;

        public SessionQuestionsController(CapstoneQuizContext context)
        {
            _context = context;
        }

        // GET: api/SessionQuestions?sessionId=5
        // includeAnswered is used for reporting purposes. Otherwise, we only need those with a status of 1 (unanswered)
        [HttpGet]
        public async Task<ActionResult<List<SessionQuestionDTO>>> GetSessionQuestion(long sessionId, string includeAnswered = "false")
        {
            var SessionQuestions = await (_context.SessionQuestion
                                            .Include(q => q.Question)
                                                .ThenInclude(ans => ans.Answers)
                                            .Include(rt => rt.ResultType)
                                            .Where(sq => sq.TestSessionId == sessionId)
                                            .ToListAsync<SessionQuestion>());

            if (SessionQuestions.Count == 0) {
                return NotFound();
            }
            List<SessionQuestionDTO> sqList = new List<SessionQuestionDTO>();
            if (includeAnswered.Equals("true")) {
                foreach (SessionQuestion sq in SessionQuestions) {
                    sqList.Add(SessionQuestionToDTO(sq));
                }
            }
            else {
                foreach (SessionQuestion sq in SessionQuestions) {
                    if (sq.ResultType.ResultTypeDescription.Equals("Unanswered")) {
                        sqList.Add(SessionQuestionToDTO(sq));
                    }
                }
            }
            int n = sqList.Count;
            if (n == 0) {
                await ensureClosedSession(sessionId, _context);
                return sqList;
            }
            Random rng = new Random();
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                SessionQuestionDTO value = sqList[k];
                sqList[k] = sqList[n];
                sqList[n] = value;
            }
            return sqList;
        }

        // PUT: api/SessionQuestions/5?action=requeue&count=2 -OR- ?action=answer
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSessionQuestion(long id, string action, long count = 0)
        {
            var SessionQuestion = await _context.SessionQuestion.FindAsync(id);
            if (SessionQuestion == null) {
                return NotFound();
            }
            if (action.Equals("requeue") && count > 0 && count <= 3) {
                SessionQuestion.ResultTypeId = 2;
                _context.Entry(SessionQuestion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                // Cap it at the required number of attempts; if you miss a question twice in a row, should still only be 3
                var existingCount = fetchExistingQuestionCount(SessionQuestion);
                for (int i = 0; i < count - existingCount; i++) {
                    var NewSessionQuestion = new SessionQuestion
                    {
                        QuestionId = SessionQuestion.QuestionId,
                        ResultTypeId = 1,
                        TestSessionId = SessionQuestion.TestSessionId
                    };
                    _context.SessionQuestion.Add(NewSessionQuestion);
                    await _context.SaveChangesAsync();
                }

            }
            else if (action.Equals("answer")) {
                SessionQuestion.ResultTypeId = 3;
                _context.Entry(SessionQuestion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else {
                return BadRequest("Unable to determine a proper action to take");
            }

            return NoContent();
        }

        // POST: api/SessionQuestions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SessionQuestionDTO>> PostSessionQuestion(SessionQuestionDTO SessionQuestionDTO)
        {
            var SessionQuestion = CreateFromDTO(SessionQuestionDTO);
            _context.SessionQuestion.Add(SessionQuestion);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetSessionQuestion", new { id = SessionQuestion.Id }, SessionQuestion);
            return CreatedAtAction(nameof(GetSessionQuestion), new { id = SessionQuestion.Id }, SessionQuestionToDTO(SessionQuestion));
        }

        // DELETE: api/SessionQuestions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SessionQuestion>> DeleteSessionQuestion(long id)
        {
            var SessionQuestion = await _context.SessionQuestion.FindAsync(id);
            if (SessionQuestion == null) {
                return NotFound();
            }

            _context.SessionQuestion.Remove(SessionQuestion);
            await _context.SaveChangesAsync();

            return SessionQuestion;
        }

        private bool SessionQuestionExists(long id)
        {
            return _context.SessionQuestion.Any(e => e.Id == id);
        }

        private static SessionQuestionDTO SessionQuestionToDTO(SessionQuestion SessionQuestion) =>
            new SessionQuestionDTO
            {
                Id = SessionQuestion.Id,
                ResultTypeId = SessionQuestion.ResultTypeId,
                QuestionId = SessionQuestion.QuestionId,
                TestSessionId = SessionQuestion.TestSessionId,
                Question = SessionQuestion.Question,
                ResultType = SessionQuestion.ResultType
            };

        private static SessionQuestion UpdatePutableFields(SessionQuestion SessionQuestion, SessionQuestionDTO SessionQuestionDTO)
        {
            SessionQuestion.QuestionId = SessionQuestionDTO.QuestionId;
            SessionQuestion.ResultTypeId = SessionQuestionDTO.ResultTypeId;
            SessionQuestion.TestSessionId = SessionQuestionDTO.TestSessionId;

            return SessionQuestion;
        }
        private static SessionQuestion CreateFromDTO(SessionQuestionDTO SessionQuestionDTO)
        {
            var SessionQuestion = new SessionQuestion
            {
                QuestionId = SessionQuestionDTO.QuestionId,
                ResultTypeId = SessionQuestionDTO.ResultTypeId,
                TestSessionId = SessionQuestionDTO.TestSessionId
            };


            return SessionQuestion;
        }

        private int fetchExistingQuestionCount(SessionQuestion sq)
        {
            var SessionQuestionsCount = _context.SessionQuestion
                                            .Include(rt => rt.ResultType)
                                            .Where(existingSq => existingSq.TestSessionId == sq.TestSessionId)
                                            .Where(existingSq => existingSq.ResultType.ResultTypeDescription.Equals("Unanswered"))
                                            .Where(existingSq => existingSq.QuestionId == sq.QuestionId)
                                            .Count();
            return SessionQuestionsCount;
        }

        private async Task ensureClosedSession(long sessionId, DbContext context)
        {
            // For use when the session returns no valid results
            var TestSession = await _context.TestSession.FindAsync(sessionId);
            if (TestSession.SessionClosedTime == null) {
                TestSession.SessionClosedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                context.Entry(TestSession).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
