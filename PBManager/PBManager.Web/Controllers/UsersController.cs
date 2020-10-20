using PBManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using AutoMapper.Configuration;
using PBManager.DAL;
using PBManager.Dto.ViewModels;
using PBManager.Services.Exensions;
using PBManager.Services.Helpers;


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
                string passwordHash = HashGenerator.GenerateHash(loginViewModel.Password);

                bool IsValidUser = _dataContext.Users.Any(
                    u => u.UserName.ToLower() == loginViewModel.UserName.ToLower() 
                    && u.Password == passwordHash
                    );

                // IEnumerable<User> us1 = _dataContext.Users.Where(us => us.UserName.ToLower() == user.UserName.ToLower()).ToList();

                if (IsValidUser)
                {
                    User u = _dataContext.Users
                        .Where(p => p.UserName.ToLower() == loginViewModel.UserName.ToLower())
                        .SingleOrDefault();

                    Session["CurrentUserName"] = u.UserName;
                    Session["CurrentUserId"] = u.Id;


                    FormsAuthentication.SetAuthCookie(u.UserName, false);




                    return RedirectToAction("Index", "Home");
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

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<RegisterViewModel, User>(); cfg.IgnoreUnmapped(); });
                IMapper mapper = config.CreateMapper();
                User user = mapper.Map<RegisterViewModel, User>(registerViewModel);
                user.Password = HashGenerator.GenerateHash(registerViewModel.Password);

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