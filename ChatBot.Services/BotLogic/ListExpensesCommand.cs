using System.Text;
using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic
{
    public class ListExpensesCommand : ICommandStrategy
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ListExpensesCommand(ITransactionRepository transactionRepo, ICategoryRepository categoryRepo)
        {
            _transactionRepo = transactionRepo;
            _categoryRepo = categoryRepo;
        }

        public string Execute(string[] args, string currentUserId)
        {
            var transactions = _transactionRepo.GetAllTransactions()
                .Where(t => t.UserId == currentUserId)
                .ToList();

            if (!transactions.Any())
                return "📭 У вас поки немає записаних витрат.";

            var sb = new StringBuilder();
            sb.AppendLine("📋 Список ваших витрат:");
            sb.AppendLine("----------------------------------");

            foreach (var t in transactions)
            {
                var category = _categoryRepo.GetCategoryById(t.CategoryId);
                string catName = category?.Name ?? "Без категорії";
                
                sb.AppendLine($"🆔 {t.Id.Substring(0, 5)} | {t.Amount} грн | {catName} | {t.Date:dd.MM.yyyy}");
            }

            sb.AppendLine("----------------------------------");
            sb.AppendLine("💡 Щоб видалити запис, введіть: /delete_expense [ID]");
            
            return sb.ToString();
        }
    }
}