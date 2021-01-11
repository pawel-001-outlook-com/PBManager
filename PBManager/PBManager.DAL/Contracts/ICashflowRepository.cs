using PBManager.Core.Models;
using System.Collections.Generic;

namespace PBManager.DAL.Contracts
{
    public interface ICashflowRepository
    {
        IEnumerable<Cashflow> GetCashflowsByUser(int userId);

        Cashflow GetCashflowById(int id);

        void Update(Cashflow cashflow);

        void Add(Cashflow cashflow);
        void Delete(Cashflow cashflow);

        int GetFilteredCount(string searchValue, int userId);
        int GetTotalCount(int userId);
        List<Cashflow> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName, int start, int length, string userId);
    }
}