using System.Text;
using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class ListExpensesCommand : ICommandStrategy
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly IExpenseRepository _expenseRepo;

        public ListExpensesCommand(ITransactionRepository transactionRepo, IExpenseRepository expenseRepo)
        {
            _transactionRepo = transactionRepo;
            _expenseRepo = expenseRepo;
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
                var category = _expenseRepo.GetCategoryById(t.CategoryId);
                string catName = category?.Name ?? "Без категорії";
                
                sb.AppendLine($"🆔 {t.Id.Substring(0, 5)} | {t.Amount} грн | {catName} | {t.Date:dd.MM.yyyy}");
            }

            sb.AppendLine("----------------------------------");
            sb.AppendLine("💡 Щоб видалити запис, введіть: /delete_expense [ID]");
            
            return sb.ToString();
        }
    }
}