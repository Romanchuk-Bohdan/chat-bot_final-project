using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface IHabitRepository
    {
        IEnumerable<Habit> GetAllHabits();
        Habit GetHabitById(string id);
        void SaveHabit(Habit habit);
        void DeleteHabit(string id);
    }
}