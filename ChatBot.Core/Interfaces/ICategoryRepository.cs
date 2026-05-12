using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(string id);
        void SaveCategory(Category category);
        void DeleteCategory(string id);
    }
}