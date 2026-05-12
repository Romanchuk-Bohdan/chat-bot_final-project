using System.Linq;
using System.Text;
using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class ListUsersCommand : ICommandStrategy
    {
        private readonly IUserRepository _userRepo;

        public ListUsersCommand(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public string Execute(string[] args, string currentUserId)
        {
            var users = _userRepo.GetAllUsers().ToList();

            if (!users.Any())
            {
                return "📭 У системі ще немає жодного користувача.";
            }

            var sb = new StringBuilder();
            sb.AppendLine("👥 Список користувачів:");
            sb.AppendLine("--------------------------");

            foreach (var user in users)
            {
                // Позначаємо стрілкою поточного активного користувача
                string currentMarker = (user.Id == currentUserId) ? "➡️ " : "  ";
                sb.AppendLine($"{currentMarker}{user.Username}");
            }

            sb.AppendLine("--------------------------");
            sb.AppendLine("💡 Щоб змінити профіль, введіть: /user_switch [Ім'я]");

            return sb.ToString();
        }
    }
}