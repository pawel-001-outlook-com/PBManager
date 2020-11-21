using PBManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PBManager.DAL;

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
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                bool IsValidUser = _dataContext.Users
                    .Any(u => u.UserName.ToLower() == user
                        .UserName.ToLower() && user
                        .Password == user.Password);

                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
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
        public ActionResult Register(User registerUser)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Users.Add(registerUser);
                _dataContext.SaveChanges();
                return RedirectToAction("Login");

            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}