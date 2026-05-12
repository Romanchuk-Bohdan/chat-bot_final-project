using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class HelpCommand : ICommandStrategy
    {
        public string Execute(string[] args, string currentUserId)
        {
            return "Доступні команди:\n" +
                   "/expense_create [сума] [категорія] - додати витрату\n" +
                   "/expense_list  - список витрат\n" +
                   "/expense_delete [ID витрати] - видалити витрату\n" +
                   "/habit_done [назва] - відмітити звичку\n" +
                   "/help - список команд\n" +
                   "/user_create [ім'я користувача] - створити нового користувача\n" +
                   "/user_rename [нове ім'я] - перейменувати теперішнього користувача" +
                   "/user_list - вивести список користувачів" +
                   "/user_switch [ім'я користувача] - зміна користувача";
        }
    }
}