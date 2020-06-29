using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CapstoneQuizAPI.Models
{
    /// <summary>
    /// Represents a single question in the UI, of any type (given a QuestionTypeId)
    /// </summary>
    public class Question
    {
        public long Id { get; set; }
        public string QuestionText { get; set; }
        /// <summary>
        ///  Represents a custom response once the question is answered
        /// </summary>
        public string QuestionExplanation { get; set; }
        public long TopicId { get; set; }
        public Topic Topic { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
