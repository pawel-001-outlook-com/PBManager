using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;

namespace PBManager.DAL.Repositories
{
    public class CashflowRepository : ICashflowRepository
    {
        private readonly DataContext _dataContext;

        public CashflowRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
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


        public void Delete(Cashflow cashflow)
        {
            _dataContext.Entry(cashflow).State = EntityState.Deleted;
        }


        public int GetFilteredCount(string searchValue)
        {
            IQueryable<Cashflow> query = _dataContext.Cashflows;

            return GetFilteredQuery(_dataContext.Cashflows, searchValue).Count();
        }


        public int GetTotalCount()
        {
            var totalCount = _dataContext.Cashflows.Count();
            return totalCount;
        }


        protected IQueryable<Cashflow> GetFilteredQuery(IQueryable<Cashflow> queryable, string searchValue)
        {
            return queryable.Where(x =>
                x.Description.Contains(searchValue)
                || x.Name.Contains(searchValue));
        }


        public List<Cashflow> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length)
        {
            List<Cashflow> result;
            if (sortDirection == "asc")
                result =
                    _dataContext.Cashflows
                        .Include(a => a.Account)
                        .Include(a => a.Category)
                        .Include(a => a.Subcategory)
                        .Include(a => a.Project)
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Value.ToString().Contains(searchValue)
                                    || a.Project.Name.Equals(searchValue)
                                    || a.Category.Name.Equals(searchValue)
                                    || a.Subcategory.Name.Equals(searchValue)
                                    || a.Account.Name.Equals(searchValue)
                        )
                        .ToList()
                        .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList();
            else
                result =
                    _dataContext.Cashflows
                        .Include(a => a.Category)
                        .Include(a => a.Subcategory)
                        .Include(a => a.Project)
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Value.ToString().Contains(searchValue)
                                    || a.Project.Name.Equals(searchValue)
                                    || a.Category.Name.Equals(searchValue)
                                    || a.Subcategory.Name.Equals(searchValue)
                                    || a.Account.Name.Equals(searchValue)
                        )
                        .ToList()
                        .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList();

            return result;
        }
    }
}