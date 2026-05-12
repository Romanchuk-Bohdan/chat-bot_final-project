using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class AddExpenseCommand : ICommandStrategy
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly ICategoryRepository _categoryRepo;

        public AddExpenseCommand(ITransactionRepository transactionRepo, ICategoryRepository categoryRepo)
        {
            _transactionRepo = transactionRepo;
            _categoryRepo = categoryRepo;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 2) 
                return "❌ Помилка! Використовуй: /expense [сума] [категорія]";

            if (!decimal.TryParse(args[0], out decimal amount))
                return "❌ Помилка! Сума має бути числом.";

            string categoryName = args[1];

            var categories = _categoryRepo.GetAllCategories();
            var category = categories.FirstOrDefault(c => 
                c.Name.ToLower() == categoryName.ToLower() && c.UserId == currentUserId);

            if (category == null)
            {
                category = new Category 
                { 
                    Name = categoryName, 
                    UserId = currentUserId, 
                    IsIncome = false 
                };
                _categoryRepo.SaveCategory(category);
            }

            var transaction = new Transaction
            {
                UserId = currentUserId,
                CategoryId = category.Id,
                Amount = amount,
                Description = $"Витрата через чат: {categoryName}"
            };

            _transactionRepo.SaveTransaction(transaction);

            return $"✅ Записано: {amount} грн у категорію '{categoryName}'.";
        }
    }
}