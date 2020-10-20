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




        // public IEnumerable<Cashflow> GetAllCashflows()
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         return context.Cashflows
        //             .Include(m => m.Account)
        //             .Include(m => m.Subcategory)
        //             .Include(m => m.Project)
        //             .ToList();
        //     }
        // }



        public IEnumerable<Cashflow> GetAllCashflows()
        {
            return _dataContext.Cashflows
                .AsNoTracking()
                .Include(m => m.Account)
                .Include(m => m.Subcategory)
                .Include(m => m.Project)
                .ToList();
        }



        // public IEnumerable<Cashflow> GetCashflowsByAccount(int accountId)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         return context.Cashflows.Where(m => m.Account.Id.Equals(accountId)).ToList();
        //     }
        // }


        public IEnumerable<Cashflow> GetCashflowsByAccount(int accountId)
        {
            return _dataContext.Cashflows
                .AsNoTracking()
                .Where(m => m.Account.Id.Equals(accountId))
                .ToList();
        }




        // public IEnumerable<Cashflow> GetCashflowsByUser(int userId)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         var t = context.Cashflows.Where(m => m.Account.UserId.Equals(userId))
        //             .Include(m => m.Account)
        //             .Include(m => m.Category)
        //             .Include(m => m.Subcategory)
        //             .ToList();
        //         return t;
        //            
        //     }
        // }



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






        // public IEnumerable<Cashflow> GetCashflowsByProject(int projectId)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         return context.Cashflows
        //             .Include(m => m.Account)
        //             .Include(m => m.Subcategory)
        //             .Include(m => m.Project)
        //             .Where(m => m.Project.Id.Equals(projectId)).ToList();
        //     }
        // }


        public IEnumerable<Cashflow> GetCashflowsByProject(int projectId)
        {
            return _dataContext.Cashflows
                .AsNoTracking()
                .Include(m => m.Account)
                .Include(m => m.Subcategory)
                .Include(m => m.Project)
                .Where(m => m.Project.Id.Equals(projectId)).ToList();
        }




        // public Cashflow GetCashflowById(int id)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         return context.Cashflows
        //             .Include(m => m.Account)
        //             .Include(m => m.Subcategory)
        //             .Include(m => m.Project)
        //             .SingleOrDefault(m => m.Id.Equals(id));
        //     }
        // }



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




        // public void Insert(Cashflow cashflow)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         context.Cashflows.Add(cashflow);
        //         context.SaveChanges();
        //     }
        // }


        public void Add(Cashflow cashflow)
        {
            _dataContext.Cashflows.Add(cashflow);
        }



        // public void Update(Cashflow cashflow)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         context.Entry(cashflow).State = EntityState.Modified;
        //         context.SaveChanges();
        //     }
        // }


        public void Update(Cashflow cashflow)
        {
            _dataContext.Entry(cashflow).State = EntityState.Modified;
        }





        // public async Task Update(IEnumerable<Cashflow> movements)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         foreach (var movement in movements)
        //             context.Entry(movement).State = EntityState.Modified;
        //         await context.SaveChangesAsync();
        //     }
        // }


        public void Update(IEnumerable<Cashflow> movements)
        {
            foreach (var movement in movements)
            {
                _dataContext.Entry(movement).State = EntityState.Modified;
            }
        }




        // public void Remove(Cashflow cashflow)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         context.Entry(cashflow).State = EntityState.Deleted;
        //         context.SaveChanges();
        //     }
        // }


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


                // if( reportViewModel.CategoryId !=null )
                // reportData = reportData.Where(m => (reportViewModel.CategoryId != null) ? m.Category.Id == (reportViewModel.CategoryId) : true);
                // reportData = reportData.Where(m => m.Category.Id.Equals(reportViewModel.CategoryId));
                // .Where(m => m.SubcategoryId.Equals(reportViewModel.SubcategoryId))
                // .Where(m => m.AccountingDate >= reportViewModel.DateFrom)
                // .Where(m => m.AccountingDate <= reportViewModel.DateTo);

                return reportData.ToList();
            }
            else
            {
                return new List<Cashflow>();
            }



            // IQueryable<Cashflow> reportData = _dataContext.Cashflows
            //
            //
            //
            //     IQueryable<Cashflow> reportData = _dataContext.Cashflows
            //         .AsNoTracking()
            //         .Include(c => c.Account)
            //         .Include(c => c.Category)
            //         .Include(c => c.Subcategory)
            //         .Include(c => c.Project)
            //         .Where(c => c.Account.Id.Equals(reportViewModel.accountId));

            // if (cfStatement.Category.HasValue)
            //     query = query.Where(m => m.Category.Id.Equals(cfStatement.Category.Value));
            // if (cfStatement.Subcategory.HasValue)
            //     query = query.Where(m => m.Subcategory.Id.Equals(cfStatement.Subcategory.Value));
            // if (cfStatement.Project.HasValue)
            //     query = query.Where(m => m.Project.Id.Equals(cfStatement.Project.Value));
            // if (cfStatement.StartDate != null)
            //     query = query.Where(m => m.AccountingDate >= cfStatement.StartDate);
            // if (cfStatement.EndDate != null)
            //     query = query.Where(m => m.AccountingDate <= cfStatement.EndDate);

            // var r = reportData.ToList();

            // return r;
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
