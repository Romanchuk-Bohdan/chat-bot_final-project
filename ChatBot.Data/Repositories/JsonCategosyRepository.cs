using System.Text.Json;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Data.Repositories
{
    public class JsonCategoryRepository : ICategoryRepository
    {
        private readonly string _filePath;

        public JsonCategoryRepository(string filePath = "categories.json")
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

        public IEnumerable<Category> GetAllCategories()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Category>>(json) ?? new List<Category>();
        }

        public Category GetCategoryById(string id)
        {
            var categories = GetAllCategories();
            return categories.FirstOrDefault(c => c.Id == id);
        }

        public void SaveCategory(Category category)
        {
            var categories = GetAllCategories().ToList();
            var existingIndex = categories.FindIndex(c => c.Id == category.Id);

            if (existingIndex >= 0)
            {
                categories[existingIndex] = category;
            }
            else
            {
                categories.Add(category);
            }

            var json = JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void DeleteCategory(string id)
        {
            var categories = GetAllCategories().ToList();
            var categoryToRemove = categories.FirstOrDefault(c => c.Id == id);
            
            if (categoryToRemove != null)
            {
                categories.Remove(categoryToRemove);
                var json = JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }
    }
}