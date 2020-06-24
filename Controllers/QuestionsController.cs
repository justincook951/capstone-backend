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
    public class QuestionsController : ControllerBase
    {
        private readonly CapstoneQuizContext _context;

        public QuestionsController(CapstoneQuizContext context)
        {
            _context = context;
        }

        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetQuestion()
        {
            return await _context.Question
                .Select(question => QuestionToDTO(question))
                .ToListAsync();
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDTO>> GetQuestion(long id)
        {
            var question = await _context.Question.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return QuestionToDTO(question);
        }

        // POST: api/Questions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> PostQuestion(QuestionDTO questionDTO)
        {
            var question = CreateFromDTO(questionDTO);
            _context.Question.Add(question);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, QuestionToDTO(question));
        }

        // PUT: api/Questions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(long id, QuestionDTO questionDTO)
        {
            if (id != questionDTO.Id) {
                return BadRequest();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null) {
                return NotFound();
            }

            question = UpdatePutableFields(question, questionDTO);

            _context.Entry(question).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!QuestionExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Question>> DeleteQuestion(long id)
        {
            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Question.Remove(question);
            await _context.SaveChangesAsync();

            return question;
        }

        private bool QuestionExists(long id)
        {
            return _context.Question.Any(e => e.Id == id);
        }

        private static QuestionDTO QuestionToDTO(Question question) =>
            new QuestionDTO
            {
                Id = question.Id,
                QuestionExplanation = question.QuestionExplanation,
                QuestionText = question.QuestionText,
                TopicId = question.TopicId
            };

        private static Question UpdatePutableFields(Question question, QuestionDTO questionDTO)
        {
            question.QuestionText = questionDTO.QuestionText;
            question.QuestionExplanation = questionDTO.QuestionExplanation;
            question.TopicId = questionDTO.TopicId;

            return question;
        }
        private static Question CreateFromDTO(QuestionDTO questionDTO)
        {
            var question = new Question
            {
                QuestionText = questionDTO.QuestionText,
                QuestionExplanation = questionDTO.QuestionExplanation,
                TopicId = questionDTO.TopicId
            };
            

            return question;
        }
    }
}
