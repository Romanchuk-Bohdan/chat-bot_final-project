using ChatBot.Core.Models;

namespace ChatBot.Core.Interfaces
{
    public interface IIncomeRepository
    {
        IEnumerable<Income> GetAllIncomes();
        Income GetIncomeById(string id);
        void SaveIncome(Income income);
        void DeleteIncome(string id);
    }
}