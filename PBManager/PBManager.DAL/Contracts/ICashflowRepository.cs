using System.Collections.Generic;
using System.Threading.Tasks;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;

namespace PBManager.DAL.Contracts
{
    public interface ICashflowRepository
    {
        IEnumerable<Cashflow> GetAllCashflows();
        IEnumerable<Cashflow> GetCashflowsByAccount(int accountId);
        IEnumerable<Cashflow> GetCashflowsByUser(int userId);
        IEnumerable<Cashflow> GetCashflowsByProject(int projectId);
        Cashflow GetCashflowById(int id);
        // void Insert(Cashflow cashflow);
        void Update(Cashflow cashflow);
        void Update(IEnumerable<Cashflow> movements);
        // void Remove(Cashflow cashflow);
        IEnumerable<Cashflow> GetAllCashflows(ReportViewModel cfStatement);
        void Add(Cashflow cashflow);
        void Delete(Cashflow cashflow);
        IEnumerable<Cashflow> GetAllCashflows(AccountChartReportViewModel reportViewModel);
    }
}