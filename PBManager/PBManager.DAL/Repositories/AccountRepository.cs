using PBManager.Core.Models;
using PBManager.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;

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
                .AsNoTracking()
                        .Include(a => a.Cashflows)
                        .ToList();
        }


        public IEnumerable<Account> GetAccounts(int userId)
        {
            return _dataContext.Accounts
                .AsNoTracking()
                .Include(a => a.Cashflows)
                .Where(c => c.UserId == userId)
                .ToList();
        }


        public Account GetAccountById(int id)
        {
            var ac = _dataContext.Accounts
                .AsNoTracking()
                .Include(a => a.Cashflows.Select(c => c.Subcategory))
                .Include(a => a.User)
                .SingleOrDefault(a => a.Id.Equals(id));
            return ac;
        }

        public Account GetAccountByIdToDelete(int id)
        {
            var ac = _dataContext.Accounts
                //.AsNoTracking()
                .Include(a => a.Cashflows.Select(c => c.Subcategory))
                .Include(a => a.User)
                .SingleOrDefault(a => a.Id.Equals(id));
            return ac;
        }

        public ICollection<Account> GetAccountsByName(string name)
        {
            return _dataContext.Accounts
                .AsNoTracking()
                .Where(a => a.Name.Equals(name)).ToList();
        }


        // ###########################################   uow


        public void Add(Account account)
        {
            _dataContext.Accounts.Add(account);
        }


        public void Delete(Account account)
        {
            var cashflowsToDelete = _dataContext.Cashflows.Where(a => a.AccountId.Equals(account.Id)).ToList();

            foreach (var c in cashflowsToDelete)
            {
                _dataContext.Entry(c).State = EntityState.Deleted;
            }

            _dataContext.Entry(account).State = EntityState.Deleted;

        }


        public void Update(Account account)
        {
            _dataContext.Entry(account).State = EntityState.Modified;
        }


        // ########################################################################################################################
        // ########################################################################################################################
        // ########################################################################################################################


        public int GetFilteredCount(string searchValue, int userId)
        {
            IQueryable<Account> query = _dataContext.Accounts;

            return GetFilteredQuery(_dataContext.Accounts, searchValue, userId).Count();
        }


        public int GetTotalCount(int userId)
        {
            int totalCount = _dataContext.Accounts
                .Where(a => a.UserId.Equals(userId))
                .Count();
            return totalCount;
        }


        protected IQueryable<Account> GetFilteredQuery(IQueryable<Account> queryable, string searchValue, int userId)
        {
            return queryable
                .Where(a => a.UserId.Equals(userId))
                .Where(x =>
                SqlFunctions.StringConvert((double)x.InitialBalance).Contains(searchValue)
                || SqlFunctions.StringConvert((double)x.Balance).Contains(searchValue)
                || x.Name.Contains(searchValue));
        }


        public List<Account> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName, int start, int length, string userId)
        {
            List<Account> result;

            int userIdInt = Convert.ToInt32(userId);

            if (sortDirection == "asc")
            {
                result =
                    _dataContext.Accounts
                        .Include(a => a.Cashflows)
                        .Where(u => u.UserId.Equals(userIdInt))
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Balance.ToString().Contains(searchValue)
                                    || a.Cashflows.Sum(c => c.Value).ToString().Contains(searchValue)
                                    )
                        .ToList<Account>()
                        .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))//Sort by sortColumn
                        .Skip(start)
                        .Take(length)
                        .ToList<Account>();
            }
            else
            {
                result =
                    _dataContext.Accounts
                        .Include(a => a.Cashflows)
                        .Where(u => u.UserId.Equals(userIdInt))
                        .Where(a => a.Name.Contains(searchValue)
                                    || a.Balance.ToString().Contains(searchValue)
                                    || a.InitialBalance.ToString().Contains(searchValue)
                        )
                        .ToList<Account>()
                        .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                        .Skip(start)
                        .Take(length)
                        .ToList<Account>();
            }

            return result;
        }

    }


}
