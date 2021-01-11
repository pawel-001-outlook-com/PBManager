namespace PBManager.DAL.Contracts
{
    public interface IUnitOfWork
    {
        IAccountRepository accounts { get; }
        ICashflowRepository cashflows { get; }
        ICategoryRepository categories { get; }
        ISubcategoryRepository subcategories { get; }
        IProjectRepository projects { get; }
        IUserRepository users { get; }
        void Complete();
    }
}