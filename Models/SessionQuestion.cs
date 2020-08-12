using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CapstoneQuizAPI.Models
{
    public class SessionQuestion
    {
        public long Id { get; set; }

        public long QuestionId { get; set; }
        public long ResultTypeId { get; set; }
        public long TestSessionId { get; set; }
        public Question Question { get; set; }
        public ResultType ResultType { get; set; }
    }
}
