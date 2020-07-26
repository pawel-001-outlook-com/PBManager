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
            return _dataContext.Accounts
                     .Include(a => a.Cashflows)
                     .ToList();
        }


        public IEnumerable<Account> GetAccounts(int userId)
        {
                return _dataContext.Accounts
                    .Include(a => a.Cashflows)
                    .Where(c => c.UserId == userId)
                    .ToList();
        }


        public Account GetAccountById(int id)
        {
                return _dataContext.Accounts
                    .Include(a => a.Cashflows.Select(c => c.Subcategory))
                    .SingleOrDefault(a => a.Id.Equals(id));
        }


        public string GetAccountNameById(int id)
        {
            return _dataContext.Accounts
                    .Where(a => a.Id.Equals(id)).Select(a => a.Name)
                    .First();
        }


        public ICollection<Account> GetAccountsByName(string name)
        {
                return _dataContext.Accounts
                    .Where(a => a.Name.Equals(name))
                    .ToList();
        }


        public Account GetAccountByName(string name)
        {
                var r = _dataContext.Accounts.Where(a => a.Name.Equals(name)).FirstOrDefault();
                return r;
        }


        public void Insert(Account account)
        {
                _dataContext.Accounts.Add(account);
                _dataContext.SaveChanges();
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

    }

    
}
