using PBManager.Core.Models;
using System.Collections.Generic;

namespace PBManager.DAL.Contracts
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAccounts();
        IEnumerable<Account> GetAccounts(int userId);
        Account GetAccountById(int id);
        Account GetAccountByIdToDelete(int id);
        ICollection<Account> GetAccountsByName(string name);

        void Update(Account account);


        void Add(Account account);
        void Delete(Account account);
        int GetTotalCount(int userId);
        int GetFilteredCount(string searchValue, int userId);
        List<Account> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName, int start, int length, string userId);
    }
}