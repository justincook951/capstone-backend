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

        // PUT: api/TestSessions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestSession(long id, TestSessionDTO TestSessionDTO)
        {
            if (id != TestSessionDTO.Id)
            {
                return BadRequest();
            }

            var TestSession = await _context.TestSession.FindAsync(id);
            if (TestSession == null) {
                return NotFound();
            }

            TestSession = UpdatePutableFields(TestSession, TestSessionDTO);

            _context.Entry(TestSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestSessionExists(id))
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

        // POST: api/TestSessions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TestSessionDTO>> PostTestSession(TestSessionDTO TestSessionDTO)
        {
            var TestSession = CreateFromDTO(TestSessionDTO);
            _context.TestSession.Add(TestSession);
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
    }
}
