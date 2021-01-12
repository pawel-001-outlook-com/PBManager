using PBManager.Core.Models;
using System.Collections.Generic;

namespace PBManager.Services.Contracts
{
    public interface ICashflowService
    {
        void Add(Cashflow cf);
        void Update(Cashflow cf);

        void Delete(int id);
        IEnumerable<Cashflow> GetAll(int userId, string uname);
        Cashflow GetById(int id);
        int GetTotalCount(int userId);
        int GetFilteredCount(string searchValue, int userId);

        List<Cashflow> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length, string userId);
    }
}