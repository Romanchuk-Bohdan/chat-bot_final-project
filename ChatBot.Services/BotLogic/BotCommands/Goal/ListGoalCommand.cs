using System.Text;
using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class ListGoalsCommand : ICommandStrategy
    {
        private readonly IGoalRepository _goalRepository;

        public ListGoalsCommand(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            var goals = _goalRepository.GetAllGoals()
                .Where(g => g.UserId == currentUserId)
                .OrderBy(g => g.IsCompleted)
                .ThenByDescending(g => g.CreatedAt)
                .ToList();

            if (!goals.Any())
                return "У вас ще немає фінансових цілей. Додайте першу: /goal_add [сума] [назва]";

            var sb = new StringBuilder();
            sb.AppendLine("🎯 **Ваші фінансові цілі:**\n");
            
            foreach (var goal in goals)
            {
                string shortId = goal.Id.Substring(0, 4);
                decimal progress = goal.TargetAmount > 0 ? (goal.CurrentAmount / goal.TargetAmount) * 100 : 0;
                string statusIcon = goal.IsCompleted ? "✅" : "⏳";

                sb.AppendLine($"{statusIcon} **{goal.Name}** (ID: {shortId})");
                sb.AppendLine($"   Зібрано: {goal.CurrentAmount} / {goal.TargetAmount} ₴ ({progress:F1}%)");
            }
            
            return sb.ToString();
        }
    }
}