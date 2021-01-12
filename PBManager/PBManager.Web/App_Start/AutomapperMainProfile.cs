using PBManager.Core.Models;
using PBManager.Dto.Dtos;
using PBManager.Dto.ViewModels;
using PBManager.Services.Exensions;

namespace PBManager.Web.App_Start
{
    public class AutomapperMainProfile : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutomapperMainProfile>();
                a.IgnoreUnmapped();
            });
        }

        public AutomapperMainProfile()
        {
            CreateMap<Account, AccountViewModel>().ReverseMap();
            CreateMap<ProjectViewModel, Project>().ReverseMap();
            CreateMap<Subcategory, SubcategoryViewModel>().ReverseMap();
            CreateMap<RegisterViewModel, User>().ReverseMap();
            CreateMap<EditUserViewModel, User>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<CashflowViewModel, Cashflow>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
        }


    }
}