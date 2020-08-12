using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CapstoneQuizAPI.Models
{
    public class TestSession
    {
        public long Id { get; set; }
        public long LastVisitedTime { get; set; }
        public long? SessionClosedTime { get; set; }
        public long UserId { get; set; }
        public long? TopicId { get; set; }
        public User User { get; set; }
        public Topic Topic { get; set; }
        public virtual ICollection<SessionQuestion> SessionQuestions { get; set; }
    }
}
