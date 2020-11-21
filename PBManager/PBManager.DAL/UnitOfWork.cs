using PBManager.DAL.Contracts;
using PBManager.DAL.Repositories;

namespace PBManager.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;


        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;

            accounts = new AccountRepository(_dataContext);
            cashflows = new CashflowRepository(_dataContext);
            categories = new CategoryRepository(_dataContext);
            subcategories = new SubcategoryRepository(_dataContext);
            projects = new ProjectRepository(_dataContext);
        }

        public IAccountRepository accounts { get; }
        public ICashflowRepository cashflows { get; }
        public ICategoryRepository categories { get; }
        public ISubcategoryRepository subcategories { get; }
        public IProjectRepository projects { get; }

        public void Complete()
        {
            _dataContext.SaveChanges();
        }
    }
}