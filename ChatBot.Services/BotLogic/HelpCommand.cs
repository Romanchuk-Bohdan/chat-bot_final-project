using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic
{
    public class HelpCommand : ICommandStrategy
    {
        public string Execute(string[] args, string currentUserId)
        {
            return "Доступні команди:\n/expense [сума] [категорія] - додати витрату\n/habit_done [назва] - відмітити звичку\n/help - список команд";
        }
    }
}