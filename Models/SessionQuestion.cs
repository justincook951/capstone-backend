﻿using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CapstoneQuizAPI.Models
{
    /// <summary>
    /// Represents a single question in the UI
    /// </summary>
    public class SessionQuestion
    {
        public long Id { get; set; }

        public long QuestionId { get; set; }
        public long ResultTypeId { get; set; }
        public long TestSessionId { get; set; }
        public TestSession TestSession { get; set; } 
        public virtual ICollection<Question> Questions { get; set; }
        public ResultType ResultType { get; set; }
    }
}
