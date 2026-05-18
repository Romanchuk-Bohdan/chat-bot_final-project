using System;
using System.Collections.Generic;
using ChatBot.Services.BotLogic;
using ChatBot.Services.BotLogic.BotCommands;
using ChatBot.Core.Interfaces;
using ChatBot.Data.Repositories;
using ChatBot.Core.Models;

namespace ChatBot.UI.Factories
{
    public static class ChatEngineFactory
    {
        // Патерн Factory: створюємо бота та всі його залежності в одному місці
        public static ChatEngine CreateEngine(IUserRepository userRepo, Action<User> onUserSwitched)
        {
            var transRepo = new JsonTransactionRepository();
            var expenseRepo = new JsonExpenseRepository();
            var habitRepo = new JsonHabitRepository();
            var incomeRepo = new JsonIncomeRepository();
            var goalRepo = new JsonGoalRepository();

            var commands = new Dictionary<string, ICommandStrategy>
            {
                { "/help",         new HelpCommand() },
                { "/help_expense", new HelpExpenseCommand() },
                { "/help_habit",   new HelpHabitCommand() },
                { "/help_user",    new HelpUserCommand() },
                { "/help_income",  new HelpIncomeCommand() },
                { "/help_goal",    new HelpGoalCommand() },

                { "/stats",  new StatsCommand(transRepo, habitRepo, expenseRepo, incomeRepo) },
                { "/report", new GenerateReportCommand(userRepo, transRepo, habitRepo) },

                { "/expense_create", new AddExpenseCommand(transRepo, expenseRepo) },
                { "/expense_list",   new ListExpensesCommand(transRepo, expenseRepo) },
                { "/expense_delete", new DeleteExpenseCommand(transRepo) },

                { "/income_add",    new AddIncomeCommand(incomeRepo) },
                { "/income_list",   new ListIncomeCommand(incomeRepo) },
                { "/income_delete", new DeleteIncomeCommand(incomeRepo) },

                { "/user_create", new CreateUserCommand(userRepo) },
                { "/user_rename", new RenameUserCommand(userRepo) },
                { "/user_list",   new ListUsersCommand(userRepo) },
                { "/user_switch", new SwitchUserCommand(userRepo, onUserSwitched) },

                { "/habit_add",    new AddHabitCommand(habitRepo) },
                { "/habit_list",   new ListHabitCommand(habitRepo) },
                { "/habit_delete", new DeleteHabitCommand(habitRepo) },

                { "/goal_add",       new AddGoalCommand(goalRepo) },
                { "/goal_list",      new ListGoalsCommand(goalRepo) },
                { "/goal_add_money", new ContributeGoalCommand(goalRepo) },
                { "/goal_delete",    new DeleteGoalCommand(goalRepo) }
            };

            return new ChatEngine(commands);
        }
    }
}
