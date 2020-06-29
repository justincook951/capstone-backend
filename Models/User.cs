using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CapstoneQuizAPI.Models
{
    /// <summary>
    /// Represents a single question in the UI, of any type (given a QuestionTypeId)
    /// </summary>
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<TestSession> TestSessions { get; set; }
    }
}
