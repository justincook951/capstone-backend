using Microsoft.Extensions.Logging;

namespace CapstoneQuizAPI.Models
{
    public class QuestionPerformance
    {
        public string TopicName { get; set; }
        public string QuestionText { get; set; }
        public int? Correct { get; set; }
        public int? Incorrect { get; set; }
    }
}
