using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using PBManager.Core.Models;
using PBManager.DAL;
using PBManager.DAL.Repositories;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Exceptions;
using PBManager.Services.Exensions;
using PBManager.Services.Helpers;

namespace PBManager.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        public ActionResult Index()
        {
            var userId = UserDataHelper.GetUserId(HttpContext);
            return View();
        }


        public ActionResult View(int id)
        {
            try
            {
                var account = _accountService.GetById(id);

                if (account.Cashflows.Count > 5) account.Cashflows = account.Cashflows.Take(5).ToList();

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Account, AccountViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var accountViewModel = mapper.Map<Account, AccountViewModel>(account);

                return PartialView(accountViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(404);
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                var account = _accountService.GetById(id);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Account, AccountViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var accountViewModel = mapper.Map<Account, AccountViewModel>(account);


                return PartialView(accountViewModel);
            }
            catch (NotFoundException e)
            {
                TempData["ErrorMessage"] = e.Message;
                return new HttpStatusCodeResult(404, e.Message);
            }
        }


        public ActionResult New()
        {
            var newAccountViewModel = new AccountViewModel();
            newAccountViewModel.UserId = (int) Session["CurrentUserId"];

            return PartialView(newAccountViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(AccountViewModel newAccountViewModel)
        {
            if (ModelState.IsValid)
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<AccountViewModel, Account>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var newAccount = mapper.Map<AccountViewModel, Account>(newAccountViewModel);

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
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "");
                }

            return View(newAccountViewModel);
        }

        public ActionResult Edit(int Id)
        {
            try
            {
                var account = _accountService.GetById(Id);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Account, AccountViewModel>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var accountViewModel = mapper.Map<Account, AccountViewModel>(account);

                return PartialView(accountViewModel);
            }
            catch (NotFoundException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, AccountViewModel accountViewModel)
        {
            if (ModelState.IsValid && Id.Equals(accountViewModel.Id) &&
                accountViewModel.UserId.Equals(UserDataHelper.GetUserId(HttpContext)))
                try
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<AccountViewModel, Account>();
                        cfg.IgnoreUnmapped();
                    });
                    var mapper = config.CreateMapper();
                    var account = mapper.Map<AccountViewModel, Account>(accountViewModel);

                    _accountService.Update(account);
                    return RedirectToAction("Index");
                }
                catch (ModelValidationException e)
                {
                    ModelState.AddModelError("AccountValidation", "");
                    return View(accountViewModel);
                }
                catch (NotFoundException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "");
                }
                catch (DbUpdateException e)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

            return View(accountViewModel);
        }


        // public JsonResult AccountData(int draw, int start, int length)
        public JsonResult AccountData()
        {
            var draw = Convert.ToInt32(Request.QueryString["draw"]);
            var start = Convert.ToInt32(Request.QueryString["start"]);
            var length = Convert.ToInt32(Request.QueryString["length"]);


            // get the column index of datatable to sort on
            var sortColumnIndex = Convert.ToInt32(Request.QueryString["order[0][column]"]);
            var sortColumnName = Request.QueryString["columns[" + sortColumnIndex + "][data]"];

            // get direction of sort
            var sortDirection = Request.QueryString["order[0][dir]"];

            //// get the search string
            var searchValue = Request.QueryString["search[value]"];


            using (var db = new DataContext())
            {
                // prevent Linq from DB calls to retrieve related entities
                db.Configuration.LazyLoadingEnabled = false;

                var repository = new AccountRepository(db);

                var recordsTotal = _accountService.GetTotalCount();
                var recordsFiltered = _accountService.GetFilteredCount(searchValue);

                var data = _accountService.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start,
                    length);


                var listObject = new List<object>();
                foreach (var p in data)
                {
                    var pr = new
                    {
                        p.Id,
                        p.Name,
                        p.InitialBalance,
                        TotalExpenses = p.Cashflows != null ? p.Cashflows.Sum(cf => cf.Value).ToString() : "0"
                    };
                    listObject.Add(pr);
                }


                var response = new
                {
                    draw,
                    recordsTotal,
                    recordsFiltered,
                    data = listObject
                };

                var jsonResponse = Json(response, JsonRequestBehavior.AllowGet);

                return jsonResponse;
            }
        }
    }
}