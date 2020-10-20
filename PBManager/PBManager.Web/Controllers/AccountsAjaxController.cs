using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;
using PBManager.Services.Exensions;

namespace PBManager.Web.Controllers
{
    public class AccountsAjaxController : Controller
    {
        private IAccountService _accountService;

        public AccountsAjaxController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        public ActionResult Index()
        {
            if (Session == null) return Redirect("~/Users/Login");
            else if (Session != null && Session["CurrentUserId"] == null) return Redirect("~/Users/Login");

            if (Session == null) RedirectToAction("Login", "Users");

            int userId = (int)Session["CurrentUserId"];


            List<AccountViewModel> accountViewModels = new List<AccountViewModel>();
            IEnumerable<Account> accounts = _accountService.GetAll(userId);


            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Account, AccountViewModel>(); cfg.IgnoreUnmapped(); });
            IMapper mapper = config.CreateMapper();
            AccountViewModel accountViewModel;


            foreach (Account a in accounts)
            {
                accountViewModel = mapper.Map<Account, AccountViewModel>(a);
                accountViewModels.Add(accountViewModel);
            }

            return PartialView(accountViewModels);
        }


        public ActionResult View(int id)
        {
            try
            {
                var account = _accountService.GetById(id);

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Account, AccountViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                AccountViewModel accountViewModel = mapper.Map<Account, AccountViewModel>(account);

                return PartialView(accountViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(404, e.Message);
            }
        }
    }
}