using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class AddExpenseCommand : ICommandStrategy
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly IExpenseRepository _expenseRepo;

        public AddExpenseCommand(ITransactionRepository transactionRepo, IExpenseRepository expenseRepo)
        {
            _transactionRepo = transactionRepo;
            _expenseRepo = expenseRepo;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 2) 
                return "❌ Помилка! Використовуй: /expense [сума] [категорія]";

            if (!decimal.TryParse(args[0], out decimal amount))
                return "❌ Помилка! Сума має бути числом.";

            string categoryName = args[1];

            var categories = _expenseRepo.GetAllCategories();
            var category = categories.FirstOrDefault(c => 
                c.Name.ToLower() == categoryName.ToLower() && c.UserId == currentUserId);

            if (category == null)
            {
                category = new Category 
                { 
                    Name = categoryName, 
                    UserId = currentUserId
                };
                _expenseRepo.SaveCategory(category);
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