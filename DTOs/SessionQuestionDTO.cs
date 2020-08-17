using CapstoneQuizAPI.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CapstoneQuizAPI.DTOs
{
    /// <summary>
    /// Represents a single question in the UI
    /// </summary>
    public class SessionQuestionDTO
    {
        public long Id { get; set; }
        public long ResultTypeId { get; set; }
        public long TestSessionId { get; set; }
        public long QuestionId { get; set; }
        public ResultType ResultType { get; set; }
        public virtual Question Question { get; set; }
    }
}
