using System;

namespace ChatBot.Core.Models
{
    public class Goal
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsCompleted => CurrentAmount >= TargetAmount;
    }
}