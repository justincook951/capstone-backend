using Microsoft.Extensions.Logging;

namespace CapstoneQuizAPI.Models
{
    /// <summary>
    /// Represents a single question in the UI, of any type (given a QuestionTypeId)
    /// </summary>
    public class ResultType
    {
        public long Id { get; set; }
        public string ResultTypeDescription { get; set; }
    }
}
