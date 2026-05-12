using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class SwitchUserCommand : ICommandStrategy
    {
        private readonly IUserRepository _userRepository;
        private readonly Action<User> _onUserSwitched;

        public SwitchUserCommand(IUserRepository userRepository, Action<User> onUserSwitched)
        {
            _userRepository = userRepository;
            _onUserSwitched = onUserSwitched;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 1)
                return "Помилка: вкажіть ім'я користувача. Приклад: /user_switch Admin";

            string targetName = args[0];
            var user = _userRepository.GetAllUsers()
                .FirstOrDefault(u => u.Username.Equals(targetName, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return $"Користувача з ім'ям '{targetName}' не знайдено.";

            _onUserSwitched?.Invoke(user);

            return $"Профіль змінено. Тепер ви: **{user.Username}**";
        }
    }
}