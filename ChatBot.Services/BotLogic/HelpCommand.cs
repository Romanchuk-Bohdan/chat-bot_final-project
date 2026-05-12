using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic
{
    public class HelpCommand : ICommandStrategy
    {
        public string Execute(string[] args, string currentUserId)
        {
            return "Доступні команди:\n" +
                   "/new_expense [сума] [категорія] - додати витрату\n" +
                   "/list_expenses  - список витрат\n" +
                   "/delete_expense [ID витрати] - видалити витрату\n" +
                   "/habit_done [назва] - відмітити звичку\n" +
                   "/help - список команд\n";
        }
    }
}