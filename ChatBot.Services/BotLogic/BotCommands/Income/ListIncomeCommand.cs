using System.Text;
using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class ListIncomeCommand : ICommandStrategy
    {
        private readonly IIncomeRepository _incomeRepository;

        public ListIncomeCommand(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository;
        }

        public string Execute(string[] args, string currentUserId)
        {
            var incomes = _incomeRepository.GetAllIncomes()
                .Where(i => i.UserId == currentUserId)
                .OrderByDescending(i => i.Date)
                .ToList();

            if (!incomes.Any())
                return "У вас ще немає записів про доходи.";

            var sb = new StringBuilder();
            sb.AppendLine("💵 **Ваші доходи:**");
            
            foreach (var inc in incomes)
            {
                string shortId = inc.Id.Substring(0, 4);
                sb.AppendLine($"#️⃣ {shortId} | {inc.Date:dd.MM.yyyy} | +{inc.Amount} ₴ | {inc.Source}");
            }
            
            sb.AppendLine($"\n**Загальна сума:** {incomes.Sum(i => i.Amount)} ₴");
            return sb.ToString();
        }
    }
}