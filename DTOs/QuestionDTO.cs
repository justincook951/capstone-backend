using CapstoneQuizAPI.Models;
using System.Collections.Generic;

namespace CapstoneQuizAPI.DTOs
{
    public class QuestionDTO
    {
        public long Id { get; set; }
        public string QuestionText { get; set; }
        public long TopicId { get; set; }
        public string QuestionExplanation { get; set; }
        public Topic Topic { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
