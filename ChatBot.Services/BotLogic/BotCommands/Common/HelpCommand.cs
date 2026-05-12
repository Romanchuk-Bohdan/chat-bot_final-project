using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class HelpCommand : ICommandStrategy
    {
        public string Execute(string[] args, string currentUserId)
        {
            return "🤖 **Меню допомоги:**\n\n" +
                   "Щоб дізнатися деталі, введіть:\n" +
                   "/help_expense - Команди фінансів (Витрати)\n" +
                   "/help_income - Команди фінансів (Доходи)\n" +
                   "/help_habit - Команди звичок\n" +
                   "/help_user - Команди користувачів\n" +
                   "/stats - Виведення загальної статистики\n" +
                   "/report [txt/csv] - Згенерувати файл зі звітом";
        }
    }
}