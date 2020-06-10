using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PBManager.Core.Models;

namespace PBManager.DAL.Repositories
{
    public class CashflowRepository
    {
        public IEnumerable<Cashflow> GetAllCashflows()
        {
            using (DataContext context = new DataContext())
            {
                return context.Cashflows
                    .Include(m => m.Account)
                    .Include(m => m.Subcategory)
                    .Include(m => m.Project)
                    .ToList();
            }
        }

        public IEnumerable<Cashflow> GetCashflowsByAccount(int accountId)
        {
            using (DataContext context = new DataContext())
            {
                return context.Cashflows.Where(m => m.Account.Id.Equals(accountId)).ToList();
            }
        }

        public IEnumerable<Cashflow> GetCashflowsByProject(int projectId)
        {
            using (DataContext context = new DataContext())
            {
                return context.Cashflows
                    .Include(m => m.Account)
                    .Include(m => m.Subcategory)
                    .Include(m => m.Project)
                    .Where(m => m.Project.Id.Equals(projectId)).ToList();
            }
        }
        public Cashflow GetCashflowById(int id)
        {
            using (DataContext context = new DataContext())
            {
                return context.Cashflows
                    .Include(m => m.Account)
                    .Include(m => m.Subcategory)
                    .Include(m => m.Project)
                    .SingleOrDefault(m => m.Id.Equals(id));
            }
        }

        public void Insert(Cashflow cashflow)
        {
            using (DataContext context = new DataContext())
            {
                context.Cashflows.Add(cashflow);
                context.SaveChanges();
            }
        }

        public void Update(Cashflow cashflow)
        {
            using (DataContext context = new DataContext())
            {
                context.Entry(cashflow).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public async Task Update(IEnumerable<Cashflow> movements)
        {
            using (DataContext context = new DataContext())
            {
                foreach (var movement in movements)
                    context.Entry(movement).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public void Remove(Cashflow cashflow)
        {
            using (DataContext context = new DataContext())
            {
                context.Entry(cashflow).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
