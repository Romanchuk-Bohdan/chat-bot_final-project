using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class HelpGoalCommand : ICommandStrategy
    {
        public string Execute(string[] args, string currentUserId)
        {
            return "🎯 **Фінансові цілі:**\n" +
                   "/goal_add [сума] [назва] — Створити нову ціль\n" +
                   "/goal_list — Переглянути всі цілі та прогрес\n" +
                   "/goal_add_money [ID] [сума] — Поповнити ціль\n" +
                   "/goal_delete [ID] — Видалити ціль";
        }
    }
}