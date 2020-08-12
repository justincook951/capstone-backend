using Microsoft.Extensions.Logging;

namespace CapstoneQuizAPI.Models
{
    public class Answer
    {
        public long Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public long QuestionId { get; set; }
    }
}
