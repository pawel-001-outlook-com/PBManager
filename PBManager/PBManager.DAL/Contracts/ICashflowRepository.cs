using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.DAL.Contracts
{
    public interface ICashflowRepository
    {
        IEnumerable<Cashflow> GetCashflowsByUser(int userId);

        Cashflow GetCashflowById(int id);

        void Update(Cashflow cashflow);

        void Add(Cashflow cashflow);
        void Delete(Cashflow cashflow);

        int GetFilteredCount(string searchValue);
        int GetTotalCount();
    }
}