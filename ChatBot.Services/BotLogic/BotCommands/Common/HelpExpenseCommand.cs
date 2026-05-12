using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class HelpExpenseCommand : ICommandStrategy
    {
        public string Execute(string[] args, string currentUserId)
        {
            return "💰 **Фінанси:**\n" +
                   "/expense_create [сума] [категорія] — Додати витрату\n" +
                   "/expense_list — Список витрат\n" +
                   "/expense_delete [ID] — Видалити витрату";
        }
    }
}