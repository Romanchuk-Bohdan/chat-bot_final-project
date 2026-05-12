using System.Text;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Services.Helpers
{
    public class TxtReport : IReport
    {
        public string GenerateContent(List<Transaction> transactions, List<Habit> habits, User user)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"📊 ЗВІТ КОРИСТУВАЧА: {user.Username}");
            sb.AppendLine($"📅 Дата: {DateTime.Now}\n");
            sb.AppendLine("💰 ВИТРАТИ:");
            if (!transactions.Any()) sb.AppendLine("- Немає даних");
            foreach (var t in transactions) 
                sb.AppendLine($"- {t.Date:dd.MM.yyyy} | {t.Amount} ₴ | ID категорії: {t.CategoryId}");

            sb.AppendLine("\n🌱 ЗВИЧКИ:");
            if (!habits.Any()) sb.AppendLine("- Немає активних звичок");
            foreach (var h in habits)
                sb.AppendLine($"- {h.Name} (Серія: {h.CurrentStreak} дн.)");

            return sb.ToString();
        }
    }
}