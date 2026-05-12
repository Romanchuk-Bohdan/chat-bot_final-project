using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;

namespace ChatBot.Services.BotLogic.BotCommands
{
    public class CreateUserCommand : ICommandStrategy
    {
        private readonly IUserRepository _userRepo;

        public CreateUserCommand(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public string Execute(string[] args, string currentUserId)
        {
            if (args.Length < 1)
            {
                return "❌ Помилка! Вкажіть ім'я: /user_create [Ім'я]";
            }

            string name = args[0];
            
            var newUser = new User 
            { 
                Username = name 
            };

            _userRepo.SaveUser(newUser);

            return $"✅ Користувача '{name}' успішно створено!\nТепер ви можете переключитися на нього командою: /user_switch {name}";
        }
    }
}