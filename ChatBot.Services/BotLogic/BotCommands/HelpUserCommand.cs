using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class HelpUserCommand : ICommandStrategy
    {
        public string Execute(string[] args, string currentUserId)
        {
            return "👤 **Користувачі:**\n" +
                   "/user_create [ім'я] — Створити профіль\n" +
                   "/user_rename [ім'я] — Змінити моє ім'я\n" +
                   "/user_list — Список всіх користувачів\n" +
                   "/user_switch [ім'я] — Перемкнути профіль";
        }
    }
}