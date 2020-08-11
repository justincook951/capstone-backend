using CapstoneQuizAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneQuizAPI.DTOs
{
    public class TestSessionDTO
    {
        public long Id { get; set; }
        public long LastVisitedTime { get; set; }
        public long? SessionClosedTime { get; set; }
        public long UserId { get; set; }
        public long TopicId { get; set; }
        public virtual ICollection<SessionQuestionDTO> SessionQuestions { get; set; }
    }
}
