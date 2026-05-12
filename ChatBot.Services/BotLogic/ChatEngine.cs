using ChatBot.Core.Interfaces;

namespace ChatBot.Services.BotLogic
{
    public class ChatEngine
    {
        private readonly Dictionary<string, ICommandStrategy> _commands;

        public ChatEngine(Dictionary<string, ICommandStrategy> commands)
        {
            _commands = commands;
        }

        public string ProcessMessage(string message, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(message)) return "Повідомлення порожнє.";
            
            if (!message.StartsWith("/"))
                return "Я розумію лише команди, що починаються з '/'. Введіть /help для списку доступних дій.";
            

            var parts = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var commandName = parts[0].ToLower();
            var args = parts.Skip(1).ToArray();

            if (_commands.ContainsKey(commandName))
            {
                return _commands[commandName].Execute(args, currentUserId);
            }

            return "Невідома команда. Спробуйте /help";
        }
    }
}