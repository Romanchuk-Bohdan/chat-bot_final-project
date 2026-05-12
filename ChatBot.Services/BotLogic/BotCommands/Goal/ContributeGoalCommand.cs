using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class ContributeGoalCommand : ICommandStrategy
    {
        private readonly IGoalRepository _goalRepository;

        public ContributeGoalCommand(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 2)
                return "❌ Вкажіть ID цілі та суму поповнення. Приклад: /goal_add_money a1b2 500";

            string shortId = args[0].ToLower();
            if (!decimal.TryParse(args[1], out decimal amount) || amount <= 0)
                return "❌ Сума поповнення має бути додатним числом.";

            var goal = _goalRepository.GetAllGoals()
                .FirstOrDefault(g => g.UserId == currentUserId && g.Id.ToLower().StartsWith(shortId));

            if (goal == null)
                return "❌ Ціль з таким ID не знайдено.";

            if (goal.IsCompleted)
                return "✅ Ця ціль вже повністю зібрана!";

            goal.CurrentAmount += amount;
            _goalRepository.SaveGoal(goal);

            if (goal.IsCompleted)
                return $"🎉 Вітаємо! Ви повністю зібрали суму на ціль '{goal.Name}'!";
            
            return $"💸 Ви додали {amount} ₴ до цілі '{goal.Name}'. Залишилось зібрати: {goal.TargetAmount - goal.CurrentAmount} ₴";
        }
    }
}