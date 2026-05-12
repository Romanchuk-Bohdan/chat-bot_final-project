using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class AddIncomeCommand : ICommandStrategy
    {
        private readonly IIncomeRepository _incomeRepository;

        public AddIncomeCommand(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 2)
                return "❌ Вкажіть суму та джерело. Приклад: /income_add 5000 Зарплата";

            if (!decimal.TryParse(args[0], out decimal amount) || amount <= 0)
                return "❌ Сума має бути додатним числом.";

            string source = string.Join(" ", args[1..]);

            var newIncome = new Income
            {
                UserId = currentUserId,
                Amount = amount,
                Source = source
            };

            _incomeRepository.SaveIncome(newIncome);
            return $"✅ Дохід успішно додано: {amount} ₴ (Джерело: {source})";
        }
    }
}