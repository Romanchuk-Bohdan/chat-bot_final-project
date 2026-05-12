using System.Text.Json;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Data.Repositories
{
    public class JsonUserRepository : IUserRepository
    {
        private readonly string _filePath;

        public JsonUserRepository(string filePath = "users.json")
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

        public IEnumerable<User> GetAllUsers()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public User GetUserById(string id)
        {
            var users = GetAllUsers();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public void SaveUser(User user)
        {
            var users = GetAllUsers().ToList();
            var existingUserIndex = users.FindIndex(u => u.Id == user.Id);

            if (existingUserIndex >= 0)
            {
                users[existingUserIndex] = user; // Оновлюємо існуючого
            }
            else
            {
                users.Add(user); // Додаємо нового
            }

            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void DeleteUser(string id)
        {
            var users = GetAllUsers().ToList();
            var userToRemove = users.FirstOrDefault(u => u.Id == id);
            
            if (userToRemove != null)
            {
                users.Remove(userToRemove);
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }
    }
}