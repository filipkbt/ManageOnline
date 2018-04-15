using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageOnline.Infrastructure;
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
                    var user = db.UserAccounts.SingleOrDefault(u => u.Username == userAccount.Username);
                    if (user != null)
                    {
                        ViewBag.Message = "Użytkownik o podanym loginie już istnieje.";

                    }
                    else
                    {
                        userAccount.Password = Crypto.Hash(userAccount.Password);
                        userAccount.ConfirmPassword = Crypto.Hash(userAccount.ConfirmPassword);
                        db.UserAccounts.Add(userAccount);
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
                var currentUser = db.UserAccounts.Where(u => u.Username == user.Username).FirstOrDefault();
                if (currentUser != null)
                {
                    if ((string.Compare(Crypto.Hash(user.Password), currentUser.Password) == 0))
                    {
                        System.Web.HttpContext.Current.Session["UserId"] = currentUser.UserId.ToString();
                        System.Web.HttpContext.Current.Session["Username"] = currentUser.Username.ToString();
                        System.Web.HttpContext.Current.Session["Role"] = currentUser.Role.ToString();

                        return RedirectToAction("DashboardIndex", "Dashboard");
                    }
                    else
                    {
                        ViewBag.Message = "Podane hasło jest nieprawidłowe";
                    }
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
            System.Web.HttpContext.Current.Session["Role"] = null;
            return RedirectToAction("Login", "Account");
        }

        public ActionResult EditAccount()
        {
            DbContextModel db = new DbContextModel();

            int UserId = Convert.ToInt32(Session["UserId"]);

            UserBasicModel userToEdit = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));


            return View(userToEdit);
        }



        [HttpPost]
        public ActionResult EditAccount(UserBasicModel userAfterEdit)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);

            using (DbContextModel db = new DbContextModel())
            {
                UserBasicModel user = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));

                user.MobileNumber = userAfterEdit.MobileNumber;

                user.DisplayedRole = userAfterEdit.DisplayedRole;

                user.Description = userAfterEdit.Description;

                db.Entry(user).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }

                catch
                {
                    ViewBag.MessageAfterEditProfileDetails = "Edycja danych nie powiodła się";
                }

                ViewBag.MessageAfterEditProfileDetails = "Edycja danych przebiegła pomyślnie";

                return View();
            }
        }

        public ActionResult ProfileDetails(int id)
        {
            DbContextModel db = new DbContextModel();

            if (id == null)
            {
                int userIdOwner = Convert.ToInt32(Session["UserId"]);
                UserBasicModel userDetails = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(userIdOwner));
                return View(userDetails);
            }
            else
            {
                UserBasicModel userDetails = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(id));

                return View(userDetails);
            }
        }

    }
}