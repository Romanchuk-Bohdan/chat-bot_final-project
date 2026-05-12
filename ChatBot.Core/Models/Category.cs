using System;

namespace ChatBot.Core.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string Name { get; set; }
        
        // Визначає, чи це категорія для доходів (true), чи для витрат (false)
        public bool IsIncome { get; set; } 
    }
}