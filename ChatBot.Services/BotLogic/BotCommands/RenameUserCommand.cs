using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class RenameUserCommand : ICommandStrategy
    {
        private readonly IUserRepository _userRepo;

        public RenameUserCommand(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 1)
            {
                return "❌ Помилка! Вкажіть нове ім'я: /user_rename [НовеІм'я]";
            }

            var user = _userRepo.GetUserById(currentUserId);
            if (user == null)
            {
                return "❌ Помилка: поточного користувача не знайдено в базі.";
            }

            string oldName = user.Username;
            user.Username = args[0];
            
            _userRepo.SaveUser(user);

            return $"✅ Успішно! Ви змінили ім'я з '{oldName}' на '{user.Username}'.";
        }
    }
}