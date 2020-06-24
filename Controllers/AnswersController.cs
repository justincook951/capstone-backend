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
    public class AnswersController : ControllerBase
    {
        private readonly CapstoneQuizContext _context;

        public AnswersController(CapstoneQuizContext context)
        {
            _context = context;
        }

        // GET: api/Answers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO>>> GetAnswer()
        {
            return await _context.Answer
                .Select(Answer => AnswerToDTO(Answer))
                .ToListAsync();
        }

        // GET: api/Answers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerDTO>> GetAnswer(long id)
        {
            var Answer = await _context.Answer.FindAsync(id);

            if (Answer == null)
            {
                return NotFound();
            }

            return AnswerToDTO(Answer);
        }

        // PUT: api/Answers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(long id, AnswerDTO AnswerDTO)
        {
            if (id != AnswerDTO.Id)
            {
                return BadRequest();
            }

            var Answer = await _context.Answer.FindAsync(id);
            if (Answer == null) {
                return NotFound();
            }

            Answer = UpdatePutableFields(Answer, AnswerDTO);

            _context.Entry(Answer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Answers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AnswerDTO>> PostAnswer(AnswerDTO AnswerDTO)
        {
            var Answer = CreateFromDTO(AnswerDTO);
            _context.Answer.Add(Answer);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetAnswer", new { id = Answer.Id }, Answer);
            return CreatedAtAction(nameof(GetAnswer), new { id = Answer.Id }, AnswerToDTO(Answer));
        }

        // DELETE: api/Answers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Answer>> DeleteAnswer(long id)
        {
            var Answer = await _context.Answer.FindAsync(id);
            if (Answer == null)
            {
                return NotFound();
            }

            _context.Answer.Remove(Answer);
            await _context.SaveChangesAsync();

            return Answer;
        }

        private bool AnswerExists(long id)
        {
            return _context.Answer.Any(e => e.Id == id);
        }

        private static AnswerDTO AnswerToDTO(Answer Answer) =>
            new AnswerDTO
            {
                Id = Answer.Id,
                AnswerText = Answer.AnswerText,
                IsCorrect = Answer.IsCorrect,
                QuestionId = Answer.QuestionId
            };

        private static Answer UpdatePutableFields(Answer Answer, AnswerDTO AnswerDTO)
        {
            Answer.IsCorrect = AnswerDTO.IsCorrect;
            Answer.AnswerText = AnswerDTO.AnswerText;
            Answer.QuestionId = AnswerDTO.QuestionId;

            return Answer;
        }
        private static Answer CreateFromDTO(AnswerDTO AnswerDTO)
        {
            var Answer = new Answer
            {
                IsCorrect = AnswerDTO.IsCorrect,
                AnswerText = AnswerDTO.AnswerText,
                QuestionId = AnswerDTO.QuestionId
            };
            

            return Answer;
        }
    }
}
