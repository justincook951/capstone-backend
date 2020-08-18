using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneQuizAPI.DTOs;
using CapstoneQuizAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CapstoneQuizAPI.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private readonly CapstoneQuizContext _context;

        public ReportsController(CapstoneQuizContext context)
        {
            _context = context;
        }

        // GET api/reports/questionPerformance
        [HttpGet("{reportType}")]
        public async Task<ActionResult<IEnumerable<ReportDTO>>> GetReport(string reportType)
        {
            /*
            Stored procedure:
                 select
	                TopicName,
	                QuestionText,
	                PivotTable.[2] AS "Incorrect",
	                PivotTable.[3] AS "Correct"
                from
                (
	                select
		                TopicName,
		                count(*) AS "attemptCount",
		                QuestionText,
		                ResultTypeId
		                FROM SessionQuestion
			                INNER JOIN QUESTION ON (SessionQuestion.QuestionId = Question.Id)
		                WHERE ResultTypeId != 1
		                group by questionId, QuestionText, ResultTypeId
                ) AS SourceTable PIVOT(SUM([attemptCount]) FOR [ResultTypeId] IN ([2], [3])) AS PivotTable
            */
            return await _context.QuestionPerformance
                .FromSqlRaw("EXEC SelectQuestionPerformance")
                .Select(qp => QuestionPerformanceToDTO(qp))
                .ToListAsync();
        }

        private static QuestionPerformanceDTO QuestionPerformanceToDTO(QuestionPerformance qp)
        {
            return new QuestionPerformanceDTO
            {
                CorrectAttempts = (qp.Correct == null ? 0 : qp.Correct),
                IncorrectAttempts = (qp.Incorrect == null ? 0 : qp.Incorrect),
                QuestionText = qp.QuestionText,
                TopicName = qp.TopicName
            };
        }
    }
}
