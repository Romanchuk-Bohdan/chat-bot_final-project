using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ChatBot.Services.BotLogic;
using ChatBot.UI.Models;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;
using ChatBot.Data.Repositories;
using ChatBot.UI.Factories;

namespace ChatBot.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _userInput;
        private string _currentUserId;
        private string _currentUserName;
        private readonly ChatEngine _chatEngine;
        private readonly IUserRepository _userRepo;

        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        public string UserInput
        {
            get => _userInput;
            set { _userInput = value; OnPropertyChanged(); }
        }

        public string CurrentUserName
        {
            get => _currentUserName;
            set { _currentUserName = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _userRepo = new JsonUserRepository();

            // Використовуємо фабрику замість жорсткого кодування (Extract Class / Factory Method)
            _chatEngine = ChatEngineFactory.CreateEngine(_userRepo, user =>
            {
                _currentUserId = user.Id;
                CurrentUserName = user.Username;
            });

            InitializeDefaultUser();

            Messages.Add(new ChatMessage
            {
                Sender = "Бот",
                Text = $"Привіт, {CurrentUserName}! Я твій асистент. Введи /help для списку команд.",
                IsUser = false
            });
        }

        private void InitializeDefaultUser()
        {
            var allUsers = _userRepo.GetAllUsers().ToList();

            if (!allUsers.Any())
            {
                var defaultUser = new User { Username = "Admin" };
                _userRepo.SaveUser(defaultUser);
                _currentUserId = defaultUser.Id;
                CurrentUserName = defaultUser.Username;
            }
            else
            {
                var user = allUsers.First();
                _currentUserId = user.Id;
                CurrentUserName = user.Username;
            }
        }

        public void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserInput)) return;

            string input = UserInput.Trim();
            Messages.Add(new ChatMessage { Sender = "Ви", Text = input, IsUser = true });

            string response = _chatEngine.ProcessMessage(input, _currentUserId);

            if (input.StartsWith("/user_rename"))
            {
                var user = _userRepo.GetUserById(_currentUserId);
                CurrentUserName = user?.Username ?? CurrentUserName;
            }

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
