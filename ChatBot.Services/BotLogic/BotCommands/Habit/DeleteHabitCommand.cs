using System.Linq;
using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class DeleteHabitCommand : ICommandStrategy
    {
        private readonly IHabitRepository _habitRepository;

        public DeleteHabitCommand(IHabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 1)
                return "Помилка: вкажіть ID звички. Приклад: /habit_delete a1b2";

            string shortId = args[0].ToLower();
            
            var habit = _habitRepository.GetAllHabits()
                .FirstOrDefault(h => h.UserId == currentUserId && h.Id.ToLower().StartsWith(shortId));

            if (habit == null)
                return "Звичку з таким ID не знайдено.";

            _habitRepository.DeleteHabit(habit.Id);
            
            return $"Звичку '{habit.Name}' видалено.";
        }
    }
}