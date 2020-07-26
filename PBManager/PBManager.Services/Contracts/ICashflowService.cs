using System.Collections.Generic;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;

namespace PBManager.Services.Contracts
{
    public interface ICashflowService
    {
        void Add(Cashflow cf);
        void Update(Cashflow cf);
        // void Update(IEnumerable<Cashflow> cashflows, bool adjustAccountBalance);
        void Delete(int id);
        IEnumerable<Cashflow> GetAll();
        IEnumerable<Cashflow> GetAll(int userId, string uname);
        IEnumerable<Cashflow> GetAll(int accountId);
        Cashflow GetById(int id);
        IEnumerable<Cashflow> GetAll(ReportViewModel bankStatement);
        IEnumerable<Cashflow> GetAll(AccountChartReportViewModel reportViewModel);
    }
}