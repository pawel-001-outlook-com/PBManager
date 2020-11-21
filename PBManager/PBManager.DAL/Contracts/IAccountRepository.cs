using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.DAL.Contracts
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAccounts();
        IEnumerable<Account> GetAccounts(int userId);
        Account GetAccountById(int id);
        ICollection<Account> GetAccountsByName(string name);

        void Update(Account account);


        void Add(Account account);
        void Delete(Account account);
        int GetTotalCount();
        int GetFilteredCount(string searchValue);

        List<Account> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName, int start,
            int length);
    }
}