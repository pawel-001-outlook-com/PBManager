using AutoMapper;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PBManager.Web.Controllers
{
    public class AccountsController : BaseController
    {
        private IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult View(int id)
        {
            var account = _accountService.GetById(id);

            if (account.Cashflows.Count > 5)
            {
                account.Cashflows = account.Cashflows.Take(5).ToList();
            }

            AccountViewModel accountViewModel = Mapper.Map<Account, AccountViewModel>(account);

            return PartialView(accountViewModel);
        }


        public ActionResult Delete(int id)
        {
            var account = _accountService.GetByIdToDelete(id);

            AccountViewModel accountViewModel = Mapper.Map<Account, AccountViewModel>(account);

            return PartialView(accountViewModel);
        }


        public ActionResult New()
        {
            var newAccountViewModel = new AccountViewModel();
            newAccountViewModel.UserId = (int)UserDataHelper.GetUserId(HttpContext);

            return PartialView(newAccountViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(AccountViewModel newAccountViewModel)
        {
            if (ModelState.IsValid)
            {
                Account newAccount = Mapper.Map<AccountViewModel, Account>(newAccountViewModel);

                _accountService.Add(newAccount);
                return RedirectToAction("Index");
            }
            else
            {
                return View(newAccountViewModel);
            }
        }

        public ActionResult Edit(int Id)
        {
            var account = _accountService.GetById(Id);

            AccountViewModel accountViewModel = Mapper.Map<Account, AccountViewModel>(account);

            return PartialView(accountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, AccountViewModel accountViewModel)
        {
            if (ModelState.IsValid && Id.Equals(accountViewModel.Id) && accountViewModel.UserId.Equals(UserDataHelper.GetUserId(HttpContext)))
            {
                Account account = Mapper.Map<AccountViewModel, Account>(accountViewModel);

                _accountService.Update(account);
                return RedirectToAction("Index");
            }
            else
            {
                return View(accountViewModel);
            }
        }



        public JsonResult AccountData(string userId = null)
        {
            int userIdInt = Convert.ToInt32(userId);

            if (userIdInt.Equals(UserDataHelper.GetUserId(HttpContext)))
            {

                int draw = Convert.ToInt32(Request.QueryString["draw"]);
                int start = Convert.ToInt32(Request.QueryString["start"]);
                int length = Convert.ToInt32(Request.QueryString["length"]);


                var sortColumnIndex = Convert.ToInt32(Request.QueryString["order[0][column]"]);
                var sortColumnName = Request.QueryString["columns[" + sortColumnIndex + "][data]"];

                var sortDirection = Request.QueryString["order[0][dir]"];

                var searchValue = Request.QueryString["search[value]"];

                var recordsTotal = _accountService.GetTotalCount(userIdInt);
                var recordsFiltered = _accountService.GetFilteredCount(searchValue, userIdInt);

                var data = _accountService.GetDataFilteredSorted(searchValue, sortDirection, sortColumnName, start,
                    length, userId);


                List<object> listObject = new List<object>();
                foreach (var p in data)
                {
                    var pr = new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        InitialBalance = p.InitialBalance.ToString("F4"),
                        TotalExpenses = p.Cashflows != null ? p.Cashflows.Sum(cf => cf.Value).ToString("F4") : "0"
                    };
                    listObject.Add(pr);
                }


                var response = new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsFiltered,
                    data = listObject
                };

                var jsonResponse = Json(response, JsonRequestBehavior.AllowGet);

                return jsonResponse;
            }
            else
            {
                throw new Exception("Bad User Id");
            }
        }


    }
}
