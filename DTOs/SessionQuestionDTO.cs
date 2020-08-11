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
        public TestSessionDTO TestSession { get; set; } 
        public virtual ICollection<QuestionDTO> Questions { get; set; }
        public ResultTypeDTO ResultType { get; set; }
    }
}
