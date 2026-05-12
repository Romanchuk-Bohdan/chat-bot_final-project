using System.Text.Json;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Data.Repositories
{
    public class JsonHabitRepository : IHabitRepository
    {
        private readonly string _filePath;

        public JsonHabitRepository(string filePath = "habits.json")
        {
            _filePath = filePath;
            EnsureFileExists();
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public IEnumerable<Habit> GetAllHabits()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Habit>>(json) ?? new List<Habit>();
        }

        public Habit GetHabitById(string id)
        {
            var habits = GetAllHabits();
            return habits.FirstOrDefault(h => h.Id == id);
        }

        public void SaveHabit(Habit habit)
        {
            var habits = GetAllHabits().ToList();
            var existingIndex = habits.FindIndex(h => h.Id == habit.Id);

            if (existingIndex >= 0)
            {
                habits[existingIndex] = habit;
            }
            else
            {
                habits.Add(habit);
            }

            var json = JsonSerializer.Serialize(habits, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void DeleteHabit(string id)
        {
            var habits = GetAllHabits().ToList();
            var habitToRemove = habits.FirstOrDefault(h => h.Id == id);
            
            if (habitToRemove != null)
            {
                habits.Remove(habitToRemove);
                var json = JsonSerializer.Serialize(habits, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }
    }
}