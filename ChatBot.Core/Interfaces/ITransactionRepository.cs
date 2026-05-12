using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAllTransactions();
        Transaction GetTransactionById(string id);
        IEnumerable<Transaction> GetTransactionsByCategory(string categoryId);
        void SaveTransaction(Transaction transaction);
        void DeleteTransaction(string id);
    }
}