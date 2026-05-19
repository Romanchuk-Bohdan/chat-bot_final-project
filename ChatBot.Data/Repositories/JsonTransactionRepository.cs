using System.Text.Json;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Data.Repositories
{
    public class JsonTransactionRepository : ITransactionRepository
    {
        private readonly string _filePath;

        public JsonTransactionRepository(string filePath = "transactions.json")
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

        private List<Transaction> LoadTransactions()
        {
            var json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<Transaction>>(json)
                   ?? new List<Transaction>();
        }

        private void SaveTransactions(List<Transaction> transactions)
        {
            var json = JsonSerializer.Serialize(
                transactions,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return LoadTransactions();
        }

        public Transaction GetTransactionById(string id)
        {
            return LoadTransactions()
                .FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<Transaction> GetTransactionsByCategory(string categoryId)
        {
            return LoadTransactions()
                .Where(t => t.CategoryId == categoryId);
        }

        public void SaveTransaction(Transaction transaction)
        {
            var transactions = LoadTransactions();

            var existingTransaction = transactions
                .FirstOrDefault(t => t.Id == transaction.Id);

            if (existingTransaction != null)
            {
                var index = transactions.IndexOf(existingTransaction);
                transactions[index] = transaction;
            }
            else
            {
                transactions.Add(transaction);
            }

            SaveTransactions(transactions);
        }

        public void DeleteTransaction(string id)
        {
            var transactions = LoadTransactions();

            var transactionToRemove = transactions
                .FirstOrDefault(t => t.Id == id);

            if (transactionToRemove == null)
                return;

            transactions.Remove(transactionToRemove);

            SaveTransactions(transactions);
        }
    }
}