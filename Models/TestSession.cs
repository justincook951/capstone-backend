using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CapstoneQuizAPI.Models
{
    /// <summary>
    /// Represents a single question in the UI, of any type (given a QuestionTypeId)
    /// </summary>
    public class TestSession
    {
        public long Id { get; set; }
        public bool SessionToken { get; set; }
        public long LastVisitedTime { get; set; }
        public long? SessionClosedTime { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<SessionQuestion> SessionQuestions { get; set; }
    }
}
