using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Newtonsoft.Json;
using PBManager.Core.Models;
using PBManager.DAL;
using PBManager.Dto.ViewModels;
using PBManager.Services.Exensions;

namespace PBManager.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly DataContext _dataContext = new DataContext();


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                // string passwordHash = HashGenerator.GenerateHash(loginViewModel.Password);
                var passwordHash =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(loginViewModel.Password, "SHA256");


                var IsValidUser = _dataContext.Users.Any(
                    u => u.UserName.ToLower() == loginViewModel.UserName.ToLower()
                         && u.Password == passwordHash
                );

                // IEnumerable<User> us1 = _dataContext.Users.Where(us => us.UserName.ToLower() == user.UserName.ToLower()).ToList();

                if (IsValidUser)
                {
                    var u = _dataContext.Users
                        .Where(p => p.UserName.ToLower() == loginViewModel.UserName.ToLower())
                        .SingleOrDefault();

                    Session["CurrentUserName"] = u.UserName;
                    Session["CurrentUserId"] = u.Id;


                    var userData = new Dictionary<string, string>();
                    userData.Add("CurrentUserName", u.UserName);
                    userData.Add("CurrentUserId", Convert.ToString(u.Id));

                    string jsonUserData;
                    jsonUserData = JsonConvert.SerializeObject(userData);


                    var ticket = new FormsAuthenticationTicket(2,
                        u.UserName,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        loginViewModel.RememberMe,
                        jsonUserData,
                        FormsAuthentication.FormsCookiePath);

                    var encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    return Redirect(
                        FormsAuthentication.GetRedirectUrl(loginViewModel.UserName, loginViewModel.RememberMe));
                }
            }

            ModelState.AddModelError("", "invalid Username or Password");
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<RegisterViewModel, User>();
                    cfg.IgnoreUnmapped();
                });
                var mapper = config.CreateMapper();
                var user = mapper.Map<RegisterViewModel, User>(registerViewModel);
                // user.Password = HashGenerator.GenerateHash(registerViewModel.Password);
                user.Password =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(registerViewModel.Password, "SHA256");

                _dataContext.Users.Add(user);
                _dataContext.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}