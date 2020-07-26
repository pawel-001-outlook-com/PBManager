using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using PBManager.Dto.ViewModels;

namespace PBManager.DAL.Repositories
{
    public class CashflowRepository : ICashflowRepository
    {
        private readonly DataContext _dataContext;

        public CashflowRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public IEnumerable<Cashflow> GetAllCashflows()
        {
            return _dataContext.Cashflows
                .AsNoTracking()
                .Include(m => m.Account)
                .Include(m => m.Subcategory)
                .Include(m => m.Project)
                .ToList();
        }


        public IEnumerable<Cashflow> GetCashflowsByAccount(int accountId)
        {
            return _dataContext.Cashflows
                .AsNoTracking()
                .Where(m => m.Account.Id.Equals(accountId))
                .ToList();
        }


        public IEnumerable<Cashflow> GetCashflowsByUser(int userId)
        {
            return _dataContext.Cashflows
                .AsNoTracking()
                .Where(m => m.Account.UserId.Equals(userId))
                .Include(m => m.Account)
                .Include(m => m.Category)
                .Include(m => m.Subcategory)
                .Include(m => m.Project)
                .ToList();
        }


        public IEnumerable<Cashflow> GetCashflowsByProject(int projectId)
        {
            return _dataContext.Cashflows
                .AsNoTracking()
                .Include(m => m.Account)
                .Include(m => m.Subcategory)
                .Include(m => m.Project)
                .Where(m => m.Project.Id.Equals(projectId)).ToList();
        }


        public Cashflow GetCashflowById(int id)
        {
            return _dataContext.Cashflows
                .AsNoTracking()
                .Include(m => m.Account)
                .Include(m => m.Category)
                .Include(m => m.Subcategory)
                .Include(m => m.Project)
                .SingleOrDefault(m => m.Id.Equals(id));
        }


        public void Add(Cashflow cashflow)
        {
            _dataContext.Cashflows.Add(cashflow);
        }


        public void Update(Cashflow cashflow)
        {
            _dataContext.Entry(cashflow).State = EntityState.Modified;
        }


        public void Update(IEnumerable<Cashflow> cashflows)
        {
            foreach (var c in cashflows)
            {
                _dataContext.Entry(c).State = EntityState.Modified;
            }
        }


        public void Delete(Cashflow cashflow)
        {
            _dataContext.Entry(cashflow).State = EntityState.Deleted;

        }




        public IEnumerable<Cashflow> GetAllCashflows(ReportViewModel reportViewModel)
        {
            IQueryable<Cashflow> reportData;

            if (reportViewModel.UserId != null && reportViewModel.DateTo.Date >= reportViewModel.DateFrom.Date)
            {
                reportData = _dataContext.Cashflows
                    .AsNoTracking()
                    .Include(c => c.Account)
                    .Include(c => c.Category)
                    .Include(c => c.Subcategory)
                    .Include(c => c.Project)
                    .Where(m => m.Account.UserId.Equals(reportViewModel.UserId))
                    .Where(m => m.Account.Id.Equals(reportViewModel.AccountId))
                    .Where(m => (reportViewModel.CategoryId != null)
                        ? m.CategoryId == (reportViewModel.CategoryId)
                        : true)
                    .Where(m => (reportViewModel.SubcategoryId != null)
                        ? m.SubcategoryId == (reportViewModel.SubcategoryId)
                        : true)
                    .Where(m => (reportViewModel.ProjectId != null)
                        ? m.ProjectId == (reportViewModel.ProjectId)
                        : true);

                return reportData.ToList();
            }
            else
            {
                return new List<Cashflow>();
            }
        }




        public IEnumerable<Cashflow> GetAllCashflows(AccountChartReportViewModel reportViewModel)
        {
            IQueryable<Cashflow> reportData;

            if (reportViewModel.UserId != null && reportViewModel.EndDate.Date >= reportViewModel.StartDate.Date)
            {
                string t = "pitstop";

                reportData = _dataContext.Cashflows
                    .AsNoTracking()
                    .Include(c => c.Account)
                    .Include(c => c.Category)
                    .Include(c => c.Subcategory)
                    .Include(c => c.Project)
                    .Where(m => m.Account.UserId.Equals(reportViewModel.UserId))
                    .Where(m => m.Account.Id.Equals(reportViewModel.AccountId))
                    .Where(m => m.AccountingDate >= reportViewModel.StartDate)
                    .Where(m => m.AccountingDate <= reportViewModel.EndDate)
                    .Where(m => (reportViewModel.CategoryId != null)
                        ? m.CategoryId == (reportViewModel.CategoryId)
                        : true)
                    .Where(m => (reportViewModel.SubcategoryId != null)
                        ? m.SubcategoryId == (reportViewModel.SubcategoryId)
                        : true)
                    .Where(m => (reportViewModel.ProjectId != null)
                        ? m.ProjectId == (reportViewModel.ProjectId)
                        : true);

                return reportData.ToList();
            }
            else
            {
                return new List<Cashflow>();
            }
        }
    }
}
