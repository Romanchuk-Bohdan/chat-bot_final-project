using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface IGoalRepository
    {
        IEnumerable<Goal> GetAllGoals();
        Goal GetGoalById(string id);
        void SaveGoal(Goal goal);
        void DeleteGoal(string id);
    }
}