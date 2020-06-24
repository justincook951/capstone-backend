using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneQuizAPI.DTOs
{
    public class QuestionDTO
    {
        public long Id { get; set; }
        public string QuestionText { get; set; }
        public long TopicId { get; set; }
        public string QuestionExplanation { get; set; }
    }
}
