using System.Text.Json;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Data.Repositories
{
    public class JsonGoalRepository : IGoalRepository
    {
        private readonly string _filePath;

        public JsonGoalRepository(string filePath = "goals.json")
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

        public IEnumerable<Goal> GetAllGoals()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Goal>>(json) ?? new List<Goal>();
        }

        public Goal GetGoalById(string id)
        {
            return GetAllGoals().FirstOrDefault(g => g.Id == id);
        }

        public void SaveGoal(Goal goal)
        {
            var goals = GetAllGoals().ToList();
            var existingIndex = goals.FindIndex(g => g.Id == goal.Id);

            if (existingIndex >= 0)
                goals[existingIndex] = goal;
            else
                goals.Add(goal);

            var json = JsonSerializer.Serialize(goals, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void DeleteGoal(string id)
        {
            var goals = GetAllGoals().ToList();
            var goalToRemove = goals.FirstOrDefault(g => g.Id == id);
            
            if (goalToRemove != null)
            {
                goals.Remove(goalToRemove);
                var json = JsonSerializer.Serialize(goals, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }
    }
}