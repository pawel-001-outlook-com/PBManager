using PBManager.Core.Models;
using System.Collections.Generic;

namespace PBManager.Services.Contracts
{
    public interface IAccountService
    {
        IEnumerable<Account> GetByUser(int userId);
        void Add(Account account);
        Account GetById(int id);

        Account GetByIdToDelete(int id);
        void Remove(int id);
        void Update(Account account);
        int GetTotalCount(int userId);
        int GetFilteredCount(string searchValue, int userId);

        List<Account> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length, string userId);
    }
}