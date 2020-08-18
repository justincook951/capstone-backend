namespace CapstoneQuizAPI.DTOs
{
    public class QuestionPerformanceDTO : ReportDTO
    {
        public string TopicName { get; set; }
        public string QuestionText { get; set; }
        public int? CorrectAttempts { get; set; }
        public int? IncorrectAttempts { get; set; }
    }
}
