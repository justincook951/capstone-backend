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
    public class TopicsController : ControllerBase
    {
        private readonly CapstoneQuizContext _context;

        public TopicsController(CapstoneQuizContext context)
        {
            _context = context;
        }

        // GET: api/Topics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopicDTO>>> GetTopic()
        {
            return await _context.Topic
                .Select(Topic => TopicToDTO(Topic))
                .ToListAsync();
        }

        // GET: api/Topics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TopicDTO>> GetTopic(long id)
        {
            var Topic = await _context.Topic.FindAsync(id);

            if (Topic == null)
            {
                return NotFound();
            }

            return TopicToDTO(Topic);
        }

        // PUT: api/Topics/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopic(long id, TopicDTO TopicDTO)
        {
            if (id != TopicDTO.Id)
            {
                return BadRequest();
            }

            var Topic = await _context.Topic.FindAsync(id);
            if (Topic == null) {
                return NotFound();
            }

            Topic = UpdatePutableFields(Topic, TopicDTO);

            _context.Entry(Topic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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

        // POST: api/Topics
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TopicDTO>> PostTopic(TopicDTO TopicDTO)
        {
            var Topic = CreateFromDTO(TopicDTO);
            _context.Topic.Add(Topic);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTopic", new { id = Topic.Id }, Topic);
            return CreatedAtAction(nameof(GetTopic), new { id = Topic.Id }, TopicToDTO(Topic));
        }

        // DELETE: api/Topics/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Topic>> DeleteTopic(long id)
        {
            var Topic = await _context.Topic.FindAsync(id);
            if (Topic == null)
            {
                return NotFound();
            }

            _context.Topic.Remove(Topic);
            await _context.SaveChangesAsync();

            return Topic;
        }

        private bool TopicExists(long id)
        {
            return _context.Topic.Any(e => e.Id == id);
        }

        private static TopicDTO TopicToDTO(Topic Topic) =>
            new TopicDTO
            {
                Id = Topic.Id,
                TopicName = Topic.TopicName,
                TopicDescription = Topic.TopicDescription,
                UserId = Topic.UserId
            };

        private static Topic UpdatePutableFields(Topic Topic, TopicDTO TopicDTO)
        {
            Topic.TopicDescription = TopicDTO.TopicDescription;
            Topic.TopicName = TopicDTO.TopicName;
            Topic.UserId = TopicDTO.UserId;

            return Topic;
        }
        private static Topic CreateFromDTO(TopicDTO TopicDTO)
        {
            var Topic = new Topic
            {
                TopicDescription = TopicDTO.TopicDescription,
                TopicName = TopicDTO.TopicName,
                UserId = TopicDTO.UserId
            };
            

            return Topic;
        }
    }
}
