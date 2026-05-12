using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class DeleteExpenseCommand : ICommandStrategy
    {
        private readonly ITransactionRepository _transactionRepo;

        public DeleteExpenseCommand(ITransactionRepository transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 1)
                return "❌ Помилка! Вкажіть ID транзакції: /delete_expense [ID]";

            string targetId = args[0];

            var transaction = _transactionRepo.GetAllTransactions()
                .FirstOrDefault(t => (t.Id == targetId || t.Id.StartsWith(targetId)) && t.UserId == currentUserId);

            if (transaction == null)
                return "⚠️ Запис із таким ID не знайдено або він вам не належить.";

            _transactionRepo.DeleteTransaction(transaction.Id);

            return $"✅ Транзакцію {transaction.Id.Substring(0, 8)}... на суму {transaction.Amount} грн успішно видалено.";
        }
    }
}