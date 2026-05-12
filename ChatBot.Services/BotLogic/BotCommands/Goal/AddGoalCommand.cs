using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class AddGoalCommand : ICommandStrategy
    {
        private readonly IGoalRepository _goalRepository;

        public AddGoalCommand(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 2)
                return "❌ Вкажіть суму та назву цілі. Приклад: /goal_add 20000 Новий телефон";

            if (!decimal.TryParse(args[0], out decimal targetAmount) || targetAmount <= 0)
                return "❌ Сума цілі має бути додатним числом.";

            string name = string.Join(" ", args[1..]);

            var newGoal = new Goal
            {
                UserId = currentUserId,
                TargetAmount = targetAmount,
                Name = name
            };

            _goalRepository.SaveGoal(newGoal);
            return $"🎯 Ціль '{name}' успішно створена! Потрібно зібрати: {targetAmount} ₴";
        }
    }
}