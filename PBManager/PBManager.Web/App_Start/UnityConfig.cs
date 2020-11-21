using System.Web.Http;
using System.Web.Mvc;
using PBManager.DAL;
using PBManager.DAL.Contracts;
using PBManager.DAL.Repositories;
using PBManager.Services.Contracts;
using PBManager.Services.Helpers;
using Unity;
using Unity.Mvc5;

namespace PBManager.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();


            container.RegisterType<IUnitOfWork, UnitOfWork>();

            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IAccountService, AccountService>();

            container.RegisterType<IProjectRepository, ProjectRepository>();
            container.RegisterType<IProjectService, ProjectService>();

            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<ICategoryService, CategoryService>();

            container.RegisterType<ISubcategoryRepository, SubcategoryRepository>();
            container.RegisterType<ISubcategoryService, SubcategoryService>();

            container.RegisterType<ICashflowRepository, CashflowRepository>();

            container.RegisterType<ICashflowService, CashflowService>();


            // container.RegisterType<ICashflowService, CashflowService>(
            //     new InjectionConstructor(
            //         new AccountService(),
            //         // container.Resolve<IAccountService>(),
            //         container.Resolve<ICashflowRepository>() 
            //         )
            //     );


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

            // // register all your components with the container here
            // // it is NOT necessary to register your controllers
            //
            // // e.g. container.RegisterType<ITestService, TestService>();
            //
            // GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}