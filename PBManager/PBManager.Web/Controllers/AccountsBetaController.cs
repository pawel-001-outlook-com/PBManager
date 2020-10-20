using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using PBManager.Core.Models;
using PBManager.DAL;
using PBManager.DAL.Contracts;
using PBManager.DAL.Repositories;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;
using PBManager.Services.Exensions;
using PBManager.Services.Helpers;
using Newtonsoft.Json;

namespace PBManager.Web.Controllers
{
    [Authorize]
    public class AccountsBetaController : Controller
    {
        private IAccountService _accountService;

        public AccountsBetaController(IAccountService accountService)
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

            return View(accountViewModels);
        }

        public ActionResult View(int id)
        {
            try
            {
                var account = _accountService.GetById(id);

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Account, AccountViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                AccountViewModel accountViewModel = mapper.Map<Account, AccountViewModel>(account);

                return View(accountViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(404, e.Message);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {

                var account = _accountService.GetById(id);

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Account, AccountViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                AccountViewModel accountViewModel = mapper.Map<Account, AccountViewModel>(account);



                return View(accountViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(404, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AccountViewModel accountViewModel)
        {

            try
            {
                if (id.Equals(accountViewModel.Id))
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<AccountViewModel, Account>();
                        cfg.IgnoreUnmapped();
                    });
                    IMapper mapper = config.CreateMapper();
                    Account account = mapper.Map<AccountViewModel, Account>(accountViewModel);


                    _accountService.Remove(id);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(accountViewModel.Id);
                }
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
            catch (DbUpdateException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }


        public ActionResult New()
        {
            var newAccountViewModel = new AccountViewModel();
            newAccountViewModel.UserId = (int)Session["CurrentUserId"];

            return View(newAccountViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(AccountViewModel newAccountViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<AccountViewModel, Account>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Account newAccount = mapper.Map<AccountViewModel, Account>(newAccountViewModel);

                    _accountService.Add(newAccount);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("AccountValidation", e.Message);
                    return View(newAccountViewModel);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(newAccountViewModel);
            }
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                var account = _accountService.GetById(Id);

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<Account, AccountViewModel>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                AccountViewModel accountViewModel = mapper.Map<Account, AccountViewModel>(account);

                return View(accountViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, AccountViewModel accountViewModel)
        {
            if (ModelState.IsValid && Id.Equals(accountViewModel.Id))
            {
                try
                {
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<AccountViewModel, Account>(); cfg.IgnoreUnmapped(); });
                    IMapper mapper = config.CreateMapper();
                    Account account = mapper.Map<AccountViewModel, Account>(accountViewModel);

                    _accountService.Update(account);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("AccountValidation", e.Message);
                    return View(accountViewModel);
                }
                catch (NotFoundException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, e.Message);
                }
                catch (DbUpdateException e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return View(accountViewModel);
            }
        }






        // public JsonResult AccountData(int draw, int start, int length)
        public JsonResult AccountData()
        {
            int draw = Convert.ToInt32(Request.QueryString["draw"]);
            int start = Convert.ToInt32(Request.QueryString["start"]);
            int length = Convert.ToInt32(Request.QueryString["length"]);






            // get the column index of datatable to sort on
            var sortColumnIndex = Convert.ToInt32(Request.QueryString["order[0][column]"]);
            // var sortColumnName = GetPersonColumnName(sortColumnIndex);
            var sortColumnName = Request.QueryString["columns[" + sortColumnIndex + "][data]"];


            // get direction of sort
            // var sortDirection = Request.QueryString["order[0][dir]"] == "asc"
            //     ? ListSortDirection.Ascending
            //     : ListSortDirection.Descending;
            var sortDirection = Request.QueryString["order[0][dir]"];



            //// get the search string
            var searchValue = Request.QueryString["search[value]"];

            using (var db = new DataContext())
            {
                db.Configuration.LazyLoadingEnabled = false; // needed as otherwise Linq will attempt DB calls to retrieve related entities

                var repository = new AccountRepository(db);

                var recordsTotal = _accountService.GetTotalCount(); 
                var recordsFiltered = _accountService.GetFilteredCount(searchValue);
                // var data = repository.GetPagedSortedFilteredList(start, length, sortColumnName, sortDirection, searchValue);
                
                var data = repository.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start, length);

                // var list = JsonConvert.SerializeObject(data,
                //     Formatting.None,
                //     new JsonSerializerSettings()
                //     {
                //         ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                //     });

                var response = new 
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsFiltered,
                    data = data
                };

                var jsonResponse = Json(response, JsonRequestBehavior.AllowGet);

                return jsonResponse;
            }

        }

    }


}