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
    public class TestSessionsController : ControllerBase
    {
        private readonly CapstoneQuizContext _context;

        public TestSessionsController(CapstoneQuizContext context)
        {
            _context = context;
        }

        // GET: api/TestSessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestSessionDTO>>> GetTestSession()
        {
            return await _context.TestSession
                .Select(TestSession => TestSessionToDTO(TestSession))
                .ToListAsync();
        }

        // GET: api/TestSessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestSessionDTO>> GetTestSession(long id)
        {
            var TestSession = await _context.TestSession.FindAsync(id);

            if (TestSession == null)
            {
                return NotFound();
            }

            return TestSessionToDTO(TestSession);
        }

        // POST: api/TestSessions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TestSessionDTO>> PostTestSession(TestSessionDTO TestSessionDTO)
        {
            var TestSession = CreateNewSession(TestSessionDTO);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTestSession", new { id = TestSession.Id }, TestSession);
            return CreatedAtAction(nameof(GetTestSession), new { id = TestSession.Id }, TestSessionToDTO(TestSession));
        }

        // DELETE: api/TestSessions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TestSession>> DeleteTestSession(long id)
        {
            var TestSession = await _context.TestSession.FindAsync(id);
            if (TestSession == null)
            {
                return NotFound();
            }

            _context.TestSession.Remove(TestSession);
            await _context.SaveChangesAsync();

            return TestSession;
        }

        private bool TestSessionExists(long id)
        {
            return _context.TestSession.Any(e => e.Id == id);
        }

        private static TestSessionDTO TestSessionToDTO(TestSession TestSession) =>
            new TestSessionDTO
            {
                Id = TestSession.Id,
                LastVisitedTime = TestSession.LastVisitedTime,
                SessionClosedTime = TestSession.SessionClosedTime,
                UserId = TestSession.UserId
            };

        private static TestSession UpdatePutableFields(TestSession TestSession, TestSessionDTO TestSessionDTO)
        {
            TestSession.LastVisitedTime = TestSessionDTO.LastVisitedTime;
            TestSession.SessionClosedTime = TestSessionDTO.SessionClosedTime;

            return TestSession;
        }
        private static TestSession CreateFromDTO(TestSessionDTO TestSessionDTO)
        {
            var TestSession = new TestSession
            {
                LastVisitedTime = TestSessionDTO.LastVisitedTime,
                UserId = TestSessionDTO.UserId
            };
            

            return TestSession;
        }

        private TestSession CreateNewSession(TestSessionDTO TestSessionDTO)
        {
            var EligibleQuestionsByTopic = GetEligibleQuestions(TestSessionDTO);
            if (EligibleQuestionsByTopic.Count < 1) {
                throw new Exception("The topic requested has no valid questions.");
            }
            TestSession session = new TestSession
            {
                LastVisitedTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UserId = TestSessionDTO.UserId
            };
            _context.TestSession.Add(session);
            _context.SaveChanges();
            var TestSessionId = session.Id;
            foreach (Question question in EligibleQuestionsByTopic) {
                SessionQuestion sQuestion = new SessionQuestion
                {
                    QuestionId = question.Id,
                    TestSessionId = TestSessionId,
                    ResultTypeId = 1
                };
                _context.SessionQuestion.Add(sQuestion);
                _context.SaveChanges();
            }
            return session;
        }

        private List<Question> GetEligibleQuestions(TestSessionDTO TestSessionDTO)
        {
            List<Question> questions = new List<Question>();
            long UserId = TestSessionDTO.UserId;
            long TopicId = TestSessionDTO.TopicId;
            var QuestionsByTopic = 
                _context.Question
                    .Select(q => new
                    {
                        QuestionId = q.Id,
                        QuestionExplanation = q.QuestionExplanation,
                        QuestionText = q.QuestionText,
                        TopicId = q.TopicId,
                        Answers = q.Answers
                            .Select(ans => new Answer{
                                Id = ans.Id,
                                AnswerText = ans.AnswerText,
                                IsCorrect = ans.IsCorrect
                            })
                            .ToList()
                    })
                    .ToList();
            foreach (var resultQuestion in QuestionsByTopic) {
                Question newQuestion = new Question
                {
                    Id = resultQuestion.QuestionId,
                    QuestionExplanation = resultQuestion.QuestionExplanation,
                    QuestionText = resultQuestion.QuestionText,
                    TopicId = resultQuestion.TopicId,
                    Answers = resultQuestion.Answers
                };
                if (testQuestionValidity(newQuestion)) {
                    questions.Add(newQuestion);
                }
            }
            return questions;
        }

        private static bool testQuestionValidity(Question untestedQuestion)
        {
            // All questions must have 2 or more answers, one of them must be marked as correct, and have text and an explanation
            bool isInvalid = true;
            if (untestedQuestion.Answers.Count < 2 
                || String.IsNullOrEmpty(untestedQuestion.QuestionExplanation)
                || String.IsNullOrEmpty(untestedQuestion.QuestionText)) {
                return isInvalid;
            }
            int correctCount = 0;
            foreach (Answer answer in untestedQuestion.Answers) {
                if (answer.IsCorrect) {
                    correctCount++;
                }
            }
            if (correctCount != 1) {
                // Either 0 or more than 1 marked as correct
                return isInvalid;
            }
            return true;
        }

    }
}
