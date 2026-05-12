using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChatBot.Services.BotLogic;
using ChatBot.UI.Models;
using ChatBot.Core.Interfaces;

namespace ChatBot.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _userInput;
        private readonly ChatEngine _chatEngine;
        
        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public string UserInput
        {
            get => _userInput;
            set { _userInput = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _chatEngine = new ChatEngine(new Dictionary<string, ICommandStrategy> 
            {
                { "/help", new HelpCommand() }
            });

            Messages.Add(new ChatMessage { Sender = "Бот", Text = "Привіт! Я твій персональний асистент. Введи /help, щоб дізнатися, що я вмію.", IsUser = false });
        }

        public void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserInput)) return;

            Messages.Add(new ChatMessage { Sender = "Ви", Text = UserInput, IsUser = true });

            string response = _chatEngine.ProcessMessage(UserInput, "1");

            Messages.Add(new ChatMessage { Sender = "Бот", Text = response, IsUser = false });

            UserInput = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}