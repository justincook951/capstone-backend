﻿using CapstoneQuizAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneQuizAPI.DTOs
{
    public class TopicDTO
    {
        public long Id { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }
        public long UserId { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
