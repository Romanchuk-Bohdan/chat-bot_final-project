using System;

namespace ChatBot.Core.Models
{
    public class Habit
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CurrentStreak { get; set; } = 0;
        public DateTime? LastCompletedDate { get; set; } 
    }
}