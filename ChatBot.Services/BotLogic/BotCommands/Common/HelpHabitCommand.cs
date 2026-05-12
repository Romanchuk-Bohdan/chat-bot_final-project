using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class HelpHabitCommand : ICommandStrategy
    {
        public string Execute(string[] args, string currentUserId)
        {
            return "🌱 **Звички:**\n" +
                   "/habit_add [назва] [опис] — Створити звичку\n" +
                   "/habit_list — Показати мої звички\n" +
                   "/habit_delete [ID] — Видалити звичку";
        }
    }
}