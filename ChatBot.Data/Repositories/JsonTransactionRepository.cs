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

        public IEnumerable<Transaction> GetAllTransactions()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }

        public Transaction GetTransactionById(string id)
        {
            var transactions = GetAllTransactions();
            return transactions.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<Transaction> GetTransactionsByCategory(string categoryId)
        {
            var transactions = GetAllTransactions();
            return transactions.Where(t => t.CategoryId == categoryId);
        }

        public void SaveTransaction(Transaction transaction)
        {
            var transactions = GetAllTransactions().ToList();
            var existingIndex = transactions.FindIndex(t => t.Id == transaction.Id);

            if (existingIndex >= 0)
            {
                transactions[existingIndex] = transaction;
            }
            else
            {
                transactions.Add(transaction);
            }

            var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void DeleteTransaction(string id)
        {
            var transactions = GetAllTransactions().ToList();
            var transactionToRemove = transactions.FirstOrDefault(t => t.Id == id);
            
            if (transactionToRemove != null)
            {
                transactions.Remove(transactionToRemove);
                var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }
    }
}