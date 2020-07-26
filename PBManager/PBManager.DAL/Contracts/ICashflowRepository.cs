using System.Collections.Generic;
using System.Threading.Tasks;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;

namespace PBManager.DAL.Contracts
{
    public interface ICashflowRepository
    {
        // mmm
        IEnumerable<Cashflow> GetAllCashflows();
        IEnumerable<Cashflow> GetCashflowsByAccount(int accountId);
        IEnumerable<Cashflow> GetCashflowsByUser(int userId);
        Cashflow GetCashflowById(int id);
        void Update(Cashflow cashflow);
        void Update(IEnumerable<Cashflow> cashflows);
        IEnumerable<Cashflow> GetAllCashflows(ReportViewModel cfStatement);
        void Add(Cashflow cashflow);
        void Delete(Cashflow cashflow);
        IEnumerable<Cashflow> GetAllCashflows(AccountChartReportViewModel reportViewModel);
    }
}