using System.Text;
using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class ListHabitCommand : ICommandStrategy
    {
        private readonly IHabitRepository _habitRepository;

        public ListHabitCommand(IHabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            var habits = _habitRepository.GetAllHabits()
                .Where(h => h.UserId == currentUserId)
                .ToList();

            if (!habits.Any())
                return "У вас поки немає доданих звичок.";

            var sb = new StringBuilder();
            sb.AppendLine("📋 Ваші звички:");
            foreach (var h in habits)
            {
                string shortId = h.Id.Substring(0, 4);
                sb.AppendLine($"#️⃣ {shortId} | {h.Name} (Серія: {h.CurrentStreak} дн.)");
                if (!string.IsNullOrEmpty(h.Description))
                    sb.AppendLine($"   📝 {h.Description}");
            }
            return sb.ToString();
        }
    }
}