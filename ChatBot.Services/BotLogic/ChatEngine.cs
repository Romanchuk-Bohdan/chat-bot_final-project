using ChatBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatBot.Services.BotLogic
{
    public class ChatEngine
    {
        private readonly Dictionary<string, ICommandStrategy> _commands;
        private const string HelpCommand = "/help";

        public ChatEngine(Dictionary<string, ICommandStrategy> commands)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public string ProcessMessage(string message, string currentSenderId)
        {
            if (string.IsNullOrWhiteSpace(message)) 
                return "Повідомлення не може бути порожнім.";

            if (!message.StartsWith("/"))
                return "Я розумію лише команди, що починаються з '/'. Введіть /help для списку доступних дій.";

            var parts = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var commandName = parts[0].ToLower();
            var args = parts.Skip(1).ToList();

            if (_commands.TryGetValue(commandName, out var command))
            {
                return command.Execute(args, currentSenderId);
            }

            // Якщо команда не знайдена, пропонуємо /help
            return $"Невідома команда '{commandName}'. Спробуйте {HelpCommand} для списку доступних команд.";
        }
    }
}
