using System.Collections.Generic;
using PBManager.Core.Models;

namespace PBManager.Services.Contracts
{
    public interface ICashflowService
    {
        void Add(Cashflow cf);
        void Update(Cashflow cf);

        void Delete(int id);
        IEnumerable<Cashflow> GetAll(int userId, string uname);
        Cashflow GetById(int id);
        int GetTotalCount();
        int GetFilteredCount(string searchValue);
    }
}