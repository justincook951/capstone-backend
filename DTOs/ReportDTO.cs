﻿namespace CapstoneQuizAPI.DTOs
{
    public class AnswerDTO
    {
        public long Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public long QuestionId { get; set; }
    }
}
