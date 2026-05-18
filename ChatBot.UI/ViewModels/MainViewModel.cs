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
            var expenseRepo = new JsonExpenseRepository();
            var habitRepo = new JsonHabitRepository();
            var incomeRepo = new JsonIncomeRepository();
            var goalRepo = new JsonGoalRepository();

            InitializeDefaultUser();

            _chatEngine = new ChatEngine(
                BuildCommands(transRepo, expenseRepo, habitRepo, incomeRepo, goalRepo));

            Messages.Add(new ChatMessage
            {
                Sender = "Бот",
                Text = $"Привіт, {CurrentUserName}! Я твій асистент. Введи /help для списку команд.",
                IsUser = false
            });
        }

        private Dictionary<string, ICommandStrategy> BuildCommands(
            JsonTransactionRepository transRepo,
            JsonExpenseRepository expenseRepo,
            JsonHabitRepository habitRepo,
            JsonIncomeRepository incomeRepo,
            JsonGoalRepository goalRepo)
        {
            return new Dictionary<string, ICommandStrategy>
            {
                { "/help",         new HelpCommand() },
                { "/help_expense", new HelpExpenseCommand() },
                { "/help_habit",   new HelpHabitCommand() },
                { "/help_user",    new HelpUserCommand() },
                { "/help_income",  new HelpIncomeCommand() },
                { "/help_goal",    new HelpGoalCommand() },

                { "/stats",  new StatsCommand(transRepo, habitRepo, expenseRepo, incomeRepo) },
                { "/report", new GenerateReportCommand(_userRepo, transRepo, habitRepo) },

                { "/expense_create", new AddExpenseCommand(transRepo, expenseRepo) },
                { "/expense_list",   new ListExpensesCommand(transRepo, expenseRepo) },
                { "/expense_delete", new DeleteExpenseCommand(transRepo) },

                { "/income_add",    new AddIncomeCommand(incomeRepo) },
                { "/income_list",   new ListIncomeCommand(incomeRepo) },
                { "/income_delete", new DeleteIncomeCommand(incomeRepo) },

                { "/user_create", new CreateUserCommand(_userRepo) },
                { "/user_rename", new RenameUserCommand(_userRepo) },
                { "/user_list",   new ListUsersCommand(_userRepo) },
                { "/user_switch", new SwitchUserCommand(_userRepo, user =>
                    {
                        _currentUserId = user.Id;
                        CurrentUserName = user.Username;
                    })
                },

                { "/habit_add",    new AddHabitCommand(habitRepo) },
                { "/habit_list",   new ListHabitCommand(habitRepo) },
                { "/habit_delete", new DeleteHabitCommand(habitRepo) },

                { "/goal_add",       new AddGoalCommand(goalRepo) },
                { "/goal_list",      new ListGoalsCommand(goalRepo) },
                { "/goal_add_money", new ContributeGoalCommand(goalRepo) },
                { "/goal_delete",    new DeleteGoalCommand(goalRepo) }
            };
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