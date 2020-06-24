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
    public class ResultTypesController : ControllerBase
    {
        private readonly CapstoneQuizContext _context;

        public ResultTypesController(CapstoneQuizContext context)
        {
            _context = context;
        }

        // GET: api/ResultTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultTypeDTO>>> GetResultType()
        {
            return await _context.ResultType
                .Select(ResultType => ResultTypeToDTO(ResultType))
                .ToListAsync();
        }

        // GET: api/ResultTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultTypeDTO>> GetResultType(long id)
        {
            var ResultType = await _context.ResultType.FindAsync(id);

            if (ResultType == null)
            {
                return NotFound();
            }

            return ResultTypeToDTO(ResultType);
        }

        // PUT: api/ResultTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResultType(long id, ResultTypeDTO ResultTypeDTO)
        {
            if (id != ResultTypeDTO.Id)
            {
                return BadRequest();
            }

            var ResultType = await _context.ResultType.FindAsync(id);
            if (ResultType == null) {
                return NotFound();
            }

            ResultType = UpdatePutableFields(ResultType, ResultTypeDTO);

            _context.Entry(ResultType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultTypeExists(id))
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

        // POST: api/ResultTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ResultTypeDTO>> PostResultType(ResultTypeDTO ResultTypeDTO)
        {
            var ResultType = CreateFromDTO(ResultTypeDTO);
            _context.ResultType.Add(ResultType);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetResultType", new { id = ResultType.Id }, ResultType);
            return CreatedAtAction(nameof(GetResultType), new { id = ResultType.Id }, ResultTypeToDTO(ResultType));
        }

        // DELETE: api/ResultTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResultType>> DeleteResultType(long id)
        {
            var ResultType = await _context.ResultType.FindAsync(id);
            if (ResultType == null)
            {
                return NotFound();
            }

            _context.ResultType.Remove(ResultType);
            await _context.SaveChangesAsync();

            return ResultType;
        }

        private bool ResultTypeExists(long id)
        {
            return _context.ResultType.Any(e => e.Id == id);
        }

        private static ResultTypeDTO ResultTypeToDTO(ResultType ResultType) =>
            new ResultTypeDTO
            {
                Id = ResultType.Id,
                ResultTypeDescription = ResultType.ResultTypeDescription,
            };

        private static ResultType UpdatePutableFields(ResultType ResultType, ResultTypeDTO ResultTypeDTO)
        {
            ResultType.ResultTypeDescription = ResultTypeDTO.ResultTypeDescription;

            return ResultType;
        }
        private static ResultType CreateFromDTO(ResultTypeDTO ResultTypeDTO)
        {
            var ResultType = new ResultType
            {
                ResultTypeDescription = ResultTypeDTO.ResultTypeDescription
            };
            

            return ResultType;
        }
    }
}
