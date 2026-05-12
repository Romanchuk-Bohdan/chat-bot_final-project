using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class AddHabitCommand : ICommandStrategy
    {
        private readonly IHabitRepository _habitRepository;

        public AddHabitCommand(IHabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 1)
                return "Помилка: вкажіть назву звички. Приклад: /habit_add Читання [опис]";

            string name = args[0];
            string description = args.Length > 1 ? string.Join(" ", args.Skip(1)) : "Без опису";

            var newHabit = new Habit
            {
                UserId = currentUserId,
                Name = name,
                Description = description,
                CurrentStreak = 0,
                LastCompletedDate = null
            };

            _habitRepository.SaveHabit(newHabit);
            
            return $"Звичку '{name}' додано. Опис: {description}";
        }
    }
}