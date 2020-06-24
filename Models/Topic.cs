﻿using Microsoft.Extensions.Logging;

namespace CapstoneQuizAPI.Models
{
    /// <summary>
    /// Represents a single question in the UI, of any type (given a QuestionTypeId)
    /// </summary>
    public class Topic
    {
        public long Id { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
        public long OwnerId { get; set; }
    }
}
