using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ChatBot.Services.BotLogic;
using ChatBot.Services.BotLogic.BotCommands;
using ChatBot.UI.Models;
using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;
using ChatBot.Data.Repositories;

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
            var transRepo = new JsonTransactionRepository();
            var catRepo = new JsonCategoryRepository();
            var habitRepo = new JsonHabitRepository();

            InitializeDefaultUser();

            var commands = new Dictionary<string, ICommandStrategy>
            {
                { "/help", new HelpCommand() },
                { "/help_expense", new HelpExpenseCommand() },
                { "/help_habit", new HelpHabitCommand() },
                { "/help_user", new HelpUserCommand() },

                { "/expense_create", new AddExpenseCommand(transRepo, catRepo) },
                { "/expense_list", new ListExpensesCommand(transRepo, catRepo) },
                { "/expense_delete", new DeleteExpenseCommand(transRepo) },

                { "/user_create", new CreateUserCommand(_userRepo) },
                { "/user_rename", new RenameUserCommand(_userRepo) },
                { "/user_list", new ListUsersCommand(_userRepo) },
                { "/user_switch", new SwitchUserCommand(_userRepo, user => 
                    {
                        _currentUserId = user.Id;
                        CurrentUserName = user.Username; 
                    }) 
                },
                
                { "/habit_add", new AddHabitCommand(habitRepo) },
                { "/habit_list", new ListHabitCommand(habitRepo) },
                { "/habit_delete", new DeleteHabitCommand(habitRepo) }
            };

            _chatEngine = new ChatEngine(commands);

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