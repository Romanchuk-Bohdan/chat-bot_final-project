using System.Text;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Services.Helpers
{
    public class CsvReport : IReport
    {
        public string GenerateContent(List<Transaction> transactions, List<Habit> habits, User user)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Type;Name;Details;Value");
            foreach (var t in transactions)
                sb.AppendLine($"Expense;{t.Amount};{t.CategoryId};{t.Date:dd.MM.yyyy}");
            foreach (var h in habits)
                sb.AppendLine($"Habit;{h.Name};Streak: {h.CurrentStreak};{h.LastCompletedDate}");
            
            return sb.ToString();
        }
    }
}