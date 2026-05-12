using System.Text;
using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class StatsCommand : ICommandStrategy
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IHabitRepository _habitRepository;
        private readonly ICategoryRepository _categoryRepository; 

        public StatsCommand(
            ITransactionRepository transactionRepository, 
            IHabitRepository habitRepository, 
            ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _habitRepository = habitRepository;
            _categoryRepository = categoryRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            var sb = new StringBuilder();
            sb.AppendLine("📊 **Ваша загальна статистика:**\n");

            var userTransactions = _transactionRepository.GetAllTransactions()
                .Where(t => t.UserId == currentUserId)
                .ToList();

            if (userTransactions.Any())
            {
                var totalExpenses = userTransactions.Sum(t => t.Amount);
                sb.AppendLine("💰 **Фінанси:**");
                sb.AppendLine($"   Загальна сума витрат: {totalExpenses} ₴");

                var topCategoryGroup = userTransactions
                    .GroupBy(t => t.CategoryId)
                    .OrderByDescending(g => g.Sum(t => t.Amount))
                    .FirstOrDefault();

                if (topCategoryGroup != null)
                {
                    var category = _categoryRepository.GetAllCategories()
                        .FirstOrDefault(c => c.Id == topCategoryGroup.Key);
                    
                    string categoryName = category != null ? category.Name : "Невідома категорія";
                    
                    sb.AppendLine($"   Найбільше витрачено на: {categoryName} ({topCategoryGroup.Sum(t => t.Amount)} ₴)");
                }
            }
            else
            {
                sb.AppendLine("💰 **Фінанси:** Немає даних про витрати.");
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
                    sb.AppendLine($"   Рекорд серії: {bestStreak} дн. (Звичка: '{bestHabit.Name}') 🔥");
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