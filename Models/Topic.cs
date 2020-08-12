using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapstoneQuizAPI.Models
{
    public class Topic
    {
        public long Id { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
