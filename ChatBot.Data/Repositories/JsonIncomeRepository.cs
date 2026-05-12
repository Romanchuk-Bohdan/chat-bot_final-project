using System.Text.Json;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Data.Repositories
{
    public class JsonIncomeRepository : IIncomeRepository
    {
        private readonly string _filePath;

        public JsonIncomeRepository(string filePath = "incomes.json")
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

        public IEnumerable<Income> GetAllIncomes()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Income>>(json) ?? new List<Income>();
        }

        public Income GetIncomeById(string id)
        {
            return GetAllIncomes().FirstOrDefault(i => i.Id == id);
        }

        public void SaveIncome(Income income)
        {
            var incomes = GetAllIncomes().ToList();
            var existingIndex = incomes.FindIndex(i => i.Id == income.Id);

            if (existingIndex >= 0)
                incomes[existingIndex] = income;
            else
                incomes.Add(income);

            var json = JsonSerializer.Serialize(incomes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void DeleteIncome(string id)
        {
            var incomes = GetAllIncomes().ToList();
            var incomeToRemove = incomes.FirstOrDefault(i => i.Id == id);
            
            if (incomeToRemove != null)
            {
                incomes.Remove(incomeToRemove);
                var json = JsonSerializer.Serialize(incomes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }
    }
}