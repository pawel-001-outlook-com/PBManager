using AutoMapper;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PBManager.Core.Models;
using PBManager.Dto.ViewModels;
using PBManager.Services.Contracts;
using PBManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PBManager.Web.Controllers
{

    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [AllowAnonymous]
        public ActionResult Login(string succes = null)
        {
            if (succes != null && succes == "true")
            {
                LoginViewModel lVM = new LoginViewModel();
                lVM.Created = true;
                return View(lVM);
            }
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(loginViewModel.Password, "SHA256");


                User ValidUser = _userService.IfValidUser(loginViewModel.UserName.ToLower(), passwordHash);

                if (ValidUser != null)
                {
                    Dictionary<string, string> userData = new Dictionary<string, string>();
                    userData.Add("CurrentUserName", ValidUser.UserName);
                    userData.Add("CurrentUserId", Convert.ToString(ValidUser.Id));

                    string jsonUserData;
                    jsonUserData = JsonConvert.SerializeObject(userData);


                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2,
                        // ValidUser.UserName,
                        ValidUser.Id.ToString(),
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        loginViewModel.RememberMe,
                        jsonUserData,
                        FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));


                    return Redirect(FormsAuthentication.GetRedirectUrl(loginViewModel.UserName, loginViewModel.RememberMe));
                }
            }
            ModelState.AddModelError("", "invalid Username or Password");
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = Mapper.Map<RegisterViewModel, User>(registerViewModel);
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(registerViewModel.Password, "SHA256");

                if (_userService.AddUser(user))
                    return RedirectToAction("Login", new { succes = "true" });

            }
            registerViewModel.CreatedSuccessfully = false;
            return View(registerViewModel);
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        public ActionResult UserNamePartial()
        {
            string username = UserDataHelper.GetUserName(HttpContext);
            int id = UserDataHelper.GetUserId(HttpContext);

            User user = _userService.GetUser(id);

            return Content(user.FirstName + " " + user.Surname);
        }

        public ActionResult UserEmailPartial()
        {
            string username = UserDataHelper.GetUserName(HttpContext);
            int id = UserDataHelper.GetUserId(HttpContext);

            User user = _userService.GetUser(id);

            return Content(user.Email);
        }


        public ActionResult Edit()
        {
            EditUserViewModel euvm =
                Mapper.Map<User, EditUserViewModel>(_userService.GetUser(UserDataHelper.GetUserId(HttpContext)));
            return View(euvm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, EditUserViewModel euvm)
        {
            if (!euvm.Id.ToString().IsNullOrWhiteSpace())
            {
                if (ModelState.IsValid && Id.Equals(euvm.Id) && euvm.Id.Equals(UserDataHelper.GetUserId(HttpContext)))
                {
                    bool flag = false;
                    User userOld = _userService.GetUser(euvm.Id.GetValueOrDefault());
                    User user = Mapper.Map<EditUserViewModel, User>(euvm);

                    if (userOld.FirstName != null
                        ? !userOld.FirstName.Equals(user.FirstName)
                          : true
                          && !user.FirstName.IsNullOrWhiteSpace())
                    {
                        userOld.FirstName = user.FirstName;
                        flag = true;
                    };

                    if (userOld.Surname != null
                        ? !userOld.Surname.Equals(user.Surname)
                        : true
                          && !user.Surname.IsNullOrWhiteSpace())
                    {
                        userOld.Surname = user.Surname;
                        flag = true;
                    };

                    if (userOld.Email != null
                        ? !userOld.Email.Equals(user.Email)
                          : true
                          && user.Email.IsNullOrWhiteSpace())
                    {
                        userOld.Email = user.Email;
                        flag = true;
                    };
                    if (!user.Password.IsNullOrWhiteSpace()
                        ? !userOld.Password.Equals(FormsAuthentication.HashPasswordForStoringInConfigFile(euvm.Password, "SHA256"))
                        : false
                    )
                    {
                        userOld.Password =
                            FormsAuthentication.HashPasswordForStoringInConfigFile(euvm.Password, "SHA256");
                        flag = true;
                    };
                    if (!userOld.UserName.Equals(user.UserName) && !user.UserName.IsNullOrWhiteSpace())
                    {
                        userOld.UserName = euvm.UserName;
                        flag = true;
                    };

                    if (flag == true)
                    {
                        bool t = _userService.Update(userOld);
                        ViewBag.EditProfileSuccess = true;
                        if (t == true) euvm.EditedSuccessfully = true;
                        if (t == false) euvm.EditedSuccessfully = false;
                    }
                    return View(euvm);
                }
                else
                {
                    return View(euvm);
                }
            }
            else
            {
                ViewBag.EditProfileSuccess = false;
                return View(euvm);
            }
        }

    }
}