using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface IReport
    {
        string GenerateContent(List<Transaction> transactions, List<Habit> habits, User user);
    }
}