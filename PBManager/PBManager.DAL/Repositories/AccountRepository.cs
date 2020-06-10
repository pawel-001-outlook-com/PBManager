using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;

namespace PBManager.DAL.Repositories
{
    public class AccountRepository
    {
        public IEnumerable<Account> GetAccounts()
        {
            using (DataContext context = new DataContext())
            {
                return context.Accounts
                    .Include(a => a.Cashflows)
                    .Include("Cashflows.Category")
                    .Where(c => c.Enabled).ToList();
            }
        }

        public Account GetAccountById(int id)
        {
            using (DataContext context = new DataContext())
            {
                return context.Accounts
                    .Include(a => a.Cashflows)
                    .Include("Cashflows.Subcategory")
                    .SingleOrDefault(a => a.Id.Equals(id));
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

        public async Task<Account> GetAccountByName(string name)
        {
            using (DataContext context = new DataContext())
            {
                return context.Accounts.Where(a => a.Name.Equals(name)).FirstOrDefault();
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

        public void Update(Account account)
        {
            using (DataContext context = new DataContext())
            {
                context.Entry(account).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

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
    }
}
