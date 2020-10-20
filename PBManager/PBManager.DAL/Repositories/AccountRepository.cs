using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            // using (DataContext context = new DataContext())
            // {
            // return context.Accounts
            return _dataContext.Accounts
         .Include(a => a.Cashflows)
                    // .Include(a => a.AccountKind)
                    //.Include("Cashflows.Category")
                    // .Where(c => c.Enabled)
                    .ToList();
            // }
        }

        public IEnumerable<Account> GetAccounts(int userId)
        {
            using (DataContext context = new DataContext())
            {
                return context.Accounts
                    .Include(a => a.Cashflows)
                    // .Include(a => a.AccountKind)
                    //.Include("Cashflows.Category")
                    // .Where(c => c.Enabled && c.UserId==userId).ToList();
                    .Where(c => c.UserId == userId)
                    .ToList();
            }
        }

        public Account GetAccountById(int id)
        {
            using (DataContext context = new DataContext())
            {
                var ac = context.Accounts
                    // .Include(a => a.AccountKind)
                    .Include(a => a.Cashflows.Select(c => c.Subcategory))
                    // .Include("Cashflows.Subcategory")
                    .SingleOrDefault(a => a.Id.Equals(id));

                // ac.AccountKinds = context.AccountKinds.ToList();

                return ac;
            }
        }

        public string GetAccountNameById(int id)
        {
            using (DataContext context = new DataContext())
            {
                return context.Accounts.Where(a => a.Id.Equals(id)).Select(a => a.Name).First();
            }
        }

        public ICollection<Account> GetAccountsByName(string name)
        {
            using (DataContext context = new DataContext())
            {
                return context.Accounts.Where(a => a.Name.Equals(name)).ToList();
            }
        }

        public Account GetAccountByName(string name)
        {
            using (DataContext context = new DataContext())
            {
                var r = context.Accounts.Where(a => a.Name.Equals(name)).FirstOrDefault();
                return r;
            }
        }

        public void Insert(Account account)
        {
            using (DataContext context = new DataContext())
            {
                context.Accounts.Add(account);
                context.SaveChanges();
            }

        }

        // public void Update(Account account)
        // {
        //     using (DataContext context = new DataContext())
        //     {
        //         context.Entry(account).State = EntityState.Modified;
        //         context.SaveChanges();
        //     }
        // }

        public void Update(IEnumerable<Account> accounts)
        {
            using (DataContext context = new DataContext())
            {
                foreach (var account in accounts)
                {
                    context.Entry(account).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public void Remove(Account account)
        {
            using (DataContext context = new DataContext())
            {
                context.Entry(account).State = EntityState.Deleted;
                context.SaveChanges();
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
                // context.SaveChanges();
        }

        public void Update(Account account)
        {
                _dataContext.Entry(account).State = EntityState.Modified;
                // context.SaveChanges();
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
            int totalCount = _dataContext.Accounts.Count();
            return totalCount;
        }


        public virtual string GetSearchPropertyName()
        {
            return null;
        }


        protected Expression<Func<Account, bool>> GetExpressionForPropertyContains(string propertyName, string value)
        {
            var parent = Expression.Parameter(typeof(Account));
            MethodInfo method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            var expressionBody = Expression.Call(Expression.Property(parent, propertyName), method, Expression.Constant(value));
            return Expression.Lambda<Func<Account, bool>>(expressionBody, parent);
        }


        public IEnumerable<Account> GetPagedSortedFilteredList(int start, int length, string orderColumnName, ListSortDirection order, string searchValue)
        {
            return CreateQueryWithWhereAndOrderBy(searchValue, orderColumnName, order)
                .Skip(start)
                .Take(length)
                .ToList();
        }

        protected virtual IQueryable<Account> CreateQueryWithWhereAndOrderBy(string searchValue, string orderColumnName, ListSortDirection order)
        {
            IQueryable<Account> query = _dataContext.Accounts;

            query = GetFilteredQuery(query, searchValue);

            query = AddOrderByToQuery(query, orderColumnName, order);

            return query;
        }

        protected virtual IQueryable<Account> AddOrderByToQuery(IQueryable<Account> query, string orderColumnName, ListSortDirection order)
        {
            var orderDirectionMethod = (order == ListSortDirection.Ascending)
                ? "OrderBy"
                : "OrderByDescending";

            var type = typeof(Account);
            var property = type.GetProperty(orderColumnName);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var filteredAndOrderedQuery = Expression.Call(typeof(Queryable), orderDirectionMethod, new Type[] { type, property.PropertyType }, query.Expression, Expression.Quote(orderByExp));

            return query.Provider.CreateQuery<Account>(filteredAndOrderedQuery);



        }




        protected IQueryable<Account> GetFilteredQuery(IQueryable<Account> queryable, string searchValue)
        {
            return queryable.Where(x =>
                // id column (int)
                SqlFunctions.StringConvert((double) x.InitialBalance).Contains(searchValue)
                || SqlFunctions.StringConvert((double) x.Balance).Contains(searchValue)
                // name column (string)
                || x.Name.Contains(searchValue));
            // date of birth column (datetime, formatted as d/M/yyyy) - limitation of sql prevented us from getting leading zeros in day or month
            // || (SqlFunctions.StringConvert((double)SqlFunctions.DatePart("dd", x.DateOfBirth)) + "/" + SqlFunctions.DatePart("mm", x.DateOfBirth) + "/" + SqlFunctions.DatePart("yyyy", x.DateOfBirth)).Contains(searchValue));
        }




        public List<Account> GetDataFilteredSorted(string searchValue, string sortDirection, string sortColumnName, int start, int length)
        {
            List<Account> result;
            if (sortDirection == "asc")
            {
                result =
                    _dataContext.Accounts
                        .Where(a => a.Name.Contains(searchValue) 
                                    || a.Balance.ToString().Contains(searchValue)
                                    || a.InitialBalance.ToString().Contains(searchValue)
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
