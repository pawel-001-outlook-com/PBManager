using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using PBManager.Core.Models;
using PBManager.DAL.Contracts;

namespace PBManager.DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _dataContext;

        public AccountRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _dataContext.Accounts
                .Include(a => a.Cashflows)
                .ToList();
        }


        public IEnumerable<Account> GetAccounts(int userId)
        {
            using (var context = new DataContext())
            {
                return context.Accounts
                    .Include(a => a.Cashflows)
                    .Where(c => c.UserId == userId)
                    .ToList();
            }
        }


        public Account GetAccountById(int id)
        {
            using (var context = new DataContext())
            {
                var ac = context.Accounts
                    .Include(a => a.Cashflows.Select(c => c.Subcategory))
                    .SingleOrDefault(a => a.Id.Equals(id));
                return ac;
            }
        }


        public ICollection<Account> GetAccountsByName(string name)
        {
            using (var context = new DataContext())
            {
                return context.Accounts.Where(a => a.Name.Equals(name)).ToList();
            }
        }


        // ###########################################   uow


        public void Add(Account account)
        {
            _dataContext.Accounts.Add(account);
        }


        public void Delete(Account account)
        {
            _dataContext.Entry(account).State = EntityState.Deleted;
        }


        public void Update(Account account)
        {
            _dataContext.Entry(account).State = EntityState.Modified;
        }


        // ########################################################################################################################
        // ########################################################################################################################
        // ########################################################################################################################


        public int GetFilteredCount(string searchValue)
        {
            IQueryable<Account> query = _dataContext.Accounts;

            return GetFilteredQuery(_dataContext.Accounts, searchValue).Count();
        }


        public int GetTotalCount()
        {
            var totalCount = _dataContext.Accounts.Count();
            return totalCount;
        }


        public List<Account> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName,
            int start, int length)
        {
            List<Account> result;
            if (sortDirection == "asc")
                result =
                    _dataContext.Accounts
                        .Include(a => a.Cashflows)
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Balance.ToString().Contains(searchValue)
                                    || a.Cashflows.Sum(c => c.Value).ToString().Contains(searchValue)
                            // || a.InitialBalance.ToString().Contains(searchValue)
                        )
                        .ToList()
                        .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)) //Sort by sortColumn
                        .Skip(start)
                        .Take(length)
                        .ToList();
            else
                result =
                    _dataContext.Accounts
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Balance.ToString().Contains(searchValue)
                                    || a.InitialBalance.ToString().Contains(searchValue)
                        )
                        .ToList()
                        .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList();

            return result;
        }


        protected IQueryable<Account> GetFilteredQuery(IQueryable<Account> queryable, string searchValue)
        {
            return queryable.Where(x =>
                SqlFunctions.StringConvert(x.InitialBalance).Contains(searchValue)
                || SqlFunctions.StringConvert(x.Balance).Contains(searchValue)
                || x.Name.Contains(searchValue));
        }
    }
}