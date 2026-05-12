using System.Text;
using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class StatsCommand : ICommandStrategy
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHabitRepository _habitRepository;
        private readonly IExpenseRepository _expenseRepository; 
        private readonly IIncomeRepository _incomeRepository;

        public StatsCommand(
            ITransactionRepository transactionRepository, 
            IHabitRepository habitRepository, 
            IExpenseRepository expenseRepository,
            IIncomeRepository incomeRepository) 
        {
            _transactionRepository = transactionRepository;
            _habitRepository = habitRepository;
            _expenseRepository = expenseRepository;
            _incomeRepository = incomeRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            var sb = new StringBuilder();
            sb.AppendLine("📊 **Ваша загальна статистика:**\n");

            var userTransactions = _transactionRepository.GetAllTransactions()
                .Where(t => t.UserId == currentUserId)
                .ToList();

            var userIncomes = _incomeRepository.GetAllIncomes()
                .Where(i => i.UserId == currentUserId)
                .ToList();

            var totalExpenses = userTransactions.Sum(t => t.Amount);
            var totalIncome = userIncomes.Sum(i => i.Amount);
            var balance = totalIncome - totalExpenses;

            sb.AppendLine("💰 **Фінанси:**");
            sb.AppendLine($"   🟩 Загальний дохід: +{totalIncome} ₴");
            sb.AppendLine($"   🟥 Загальні витрати: -{totalExpenses} ₴");
            sb.AppendLine($"   ⚖️ Баланс: {balance} ₴");

            if (userTransactions.Any())
            {
                var topCategoryGroup = userTransactions
                    .GroupBy(t => t.CategoryId)
                    .OrderByDescending(g => g.Sum(t => t.Amount))
                    .FirstOrDefault();

                if (topCategoryGroup != null)
                {
                    var category = _expenseRepository.GetAllCategories()
                        .FirstOrDefault(c => c.Id == topCategoryGroup.Key);
                    
                    string categoryName = category != null ? category.Name : "Невідома категорія";
                    
                    sb.AppendLine($"   🔥 Найбільша стаття витрат: {categoryName} ({topCategoryGroup.Sum(t => t.Amount)} ₴)");
                }
            }

            sb.AppendLine();

            var userHabits = _habitRepository.GetAllHabits()
                .Where(h => h.UserId == currentUserId)
                .ToList();

            if (userHabits.Any())
            {
                var totalHabits = userHabits.Count;
                var bestStreak = userHabits.Max(h => h.CurrentStreak);
                var bestHabit = userHabits.FirstOrDefault(h => h.CurrentStreak == bestStreak);

                sb.AppendLine("🌱 **Звички:**");
                sb.AppendLine($"   Всього активних звичок: {totalHabits}");
                if (bestStreak > 0 && bestHabit != null)
                {
                    sb.AppendLine($"   Рекорд серії: {bestStreak} дн. (Звичка: '{bestHabit.Name}') 🏆");
                }
            }
            else
            {
                sb.AppendLine("🌱 **Звички:** Немає активних звичок.");
            }

            return sb.ToString();
        }
    }
}