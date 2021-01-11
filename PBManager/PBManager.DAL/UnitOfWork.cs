using PBManager.DAL.Contracts;
using PBManager.DAL.Repositories;

namespace PBManager.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public IAccountRepository accounts { get; private set; }
        public ICashflowRepository cashflows { get; private set; }
        public ICategoryRepository categories { get; private set; }
        public ISubcategoryRepository subcategories { get; private set; }
        public IProjectRepository projects { get; private set; }
        public IUserRepository users { get; private set; }


        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;

            accounts = new AccountRepository(_dataContext);
            cashflows = new CashflowRepository(_dataContext);
            categories = new CategoryRepository(_dataContext);
            subcategories = new SubcategoryRepository(_dataContext);
            projects = new ProjectRepository(_dataContext);
            users = new UserRepository(_dataContext);
        }

        public void Complete()
        {
            _dataContext.SaveChanges();
        }
    }

}
