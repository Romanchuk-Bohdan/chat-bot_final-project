using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class DeleteIncomeCommand : ICommandStrategy
    {
        private readonly IIncomeRepository _incomeRepository;

        public DeleteIncomeCommand(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 1)
                return "❌ Вкажіть ID доходу. Приклад: /income_delete a1b2";

            string shortId = args[0].ToLower();
            var income = _incomeRepository.GetAllIncomes()
                .FirstOrDefault(i => i.UserId == currentUserId && i.Id.ToLower().StartsWith(shortId));

            if (income == null)
                return "❌ Дохід з таким ID не знайдено.";

            _incomeRepository.DeleteIncome(income.Id);
            return $"✅ Дохід з джерела '{income.Source}' на суму {income.Amount} ₴ видалено.";
        }
    }
}