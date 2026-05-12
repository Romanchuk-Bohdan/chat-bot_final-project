using ChatBot.Core.Interfaces;
using ChatBot.Services.Helpers; 

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class GenerateReportCommand : ICommandStrategy
    {
        private readonly IUserRepository _userRepo;
        private readonly ITransactionRepository _transRepo;
        private readonly IHabitRepository _habitRepo;
        private readonly ReportFactory _factory;

        public GenerateReportCommand(IUserRepository userRepo, ITransactionRepository transRepo, IHabitRepository habitRepo)
        {
            _userRepo = userRepo;
            _transRepo = transRepo;
            _habitRepo = habitRepo;
            _factory = new ReportFactory();
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 1) return "Вкажіть формат: /report txt або /report csv";
            string format = args[0].ToLower();

            try
            {
                var report = _factory.CreateReport(format);
                var user = _userRepo.GetUserById(currentUserId);
                var trans = _transRepo.GetAllTransactions().Where(t => t.UserId == currentUserId).ToList();
                var habits = _habitRepo.GetAllHabits().Where(h => h.UserId == currentUserId).ToList();

                string content = report.GenerateContent(trans, habits, user);
                string fileName = $"report_{user.Username}_{DateTime.Now:HHmmss}.{format}";
                File.WriteAllText(fileName, content);

                return $"✅ Звіт збережено у файл: {fileName}";
            }
            catch (Exception ex) { return $"❌ Помилка: {ex.Message}"; }
        }
    }
}