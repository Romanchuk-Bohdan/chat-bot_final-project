using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(string id);
        void SaveUser(User user);
        void DeleteUser(string id);
    }
}