using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class HelpIncomeCommand : ICommandStrategy
    {
        public string Execute(string[] args, string currentUserId)
        {
            return "💵 **Доходи:**\n" +
                   "/income_add [сума] [джерело] — Додати запис про дохід\n" +
                   "/income_list — Показати список усіх доходів\n" +
                   "/income_delete [ID] — Видалити дохід за ID (перші 4 символи)";
        }
    }
}