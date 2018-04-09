using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageOnline.Models;

namespace ManageOnline.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserBasicModel userAccount)
        {
            if (ModelState.IsValid)
            {
                using (DbContextModel db = new DbContextModel())
                {
                    var user = db.userAccount.SingleOrDefault(u => u.Username == userAccount.Username);
                    if (user != null)
                    {
                        ViewBag.Message = "Użytkownik o podanym loginie już istnieje.";
                       
                    }
                    else
                    {
                        db.userAccount.Add(userAccount);
                        db.SaveChanges();
                        ModelState.Clear();
                        ViewBag.Message = "Poprawnie się zarejestrowałeś " + userAccount.Username;
                    }
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserBasicModel user)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var currentUser = db.userAccount.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();
                if (currentUser != null)
                {
                    System.Web.HttpContext.Current.Session["UserId"] = currentUser.UserID.ToString();
                    System.Web.HttpContext.Current.Session["Username"] = currentUser.Username.ToString();
                    return RedirectToAction("DashboardIndex", "Dashboard");
                }
                else
                {
                    ViewBag.Message = "Podany użytkownik nie istnieje";
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            System.Web.HttpContext.Current.Session["UserId"] = null;
            System.Web.HttpContext.Current.Session["Username"] = null;
            return RedirectToAction("Login","Account");
        }

    }
}