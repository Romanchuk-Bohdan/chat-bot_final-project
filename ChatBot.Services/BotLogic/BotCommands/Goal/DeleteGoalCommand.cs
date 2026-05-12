using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class DeleteGoalCommand : ICommandStrategy
    {
        private readonly IGoalRepository _goalRepository;

        public DeleteGoalCommand(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 1)
                return "❌ Вкажіть ID цілі. Приклад: /goal_delete a1b2";

            string shortId = args[0].ToLower();
            var goal = _goalRepository.GetAllGoals()
                .FirstOrDefault(g => g.UserId == currentUserId && g.Id.ToLower().StartsWith(shortId));

            if (goal == null)
                return "❌ Ціль з таким ID не знайдено.";

            _goalRepository.DeleteGoal(goal.Id);
            return $"🗑 Ціль '{goal.Name}' видалено.";
        }
    }
}