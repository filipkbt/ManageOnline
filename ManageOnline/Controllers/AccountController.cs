using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
                        byte[] data = System.Text.Encoding.ASCII.GetBytes("0");
                        user.UserPhoto = data;
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
                        Session.Timeout = 30;
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

        public ActionResult ChangePassword()
        {
            using (DbContextModel db = new DbContextModel())
            {
                int UserId = Convert.ToInt32(Session["UserId"]);

                UserBasicModel userToEdit = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));

                return View(userToEdit);
            }             
        }

        [HttpPost]
        public ActionResult ChangePassword(string confirmOldPassword, string newPassword, string confirmNewPassword)
        {
            using (DbContextModel db = new DbContextModel())
            {
                int UserId = Convert.ToInt32(Session["UserId"]);

                UserBasicModel userToEdit = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));
                var oldPasswordHashed = Crypto.Hash(confirmOldPassword);

                if((string.Compare(userToEdit.Password, oldPasswordHashed) == 0))
                {
                    if(newPassword == confirmNewPassword)
                    {
                        userToEdit.Password = Crypto.Hash(newPassword);
                        userToEdit.ConfirmPassword = Crypto.Hash(confirmNewPassword);
                        db.Entry(userToEdit).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.MessageInfoRight = "Hasło zostało zmienione.";
                        return View();
                    }
                    else
                    {
                        ViewBag.MessageInfoWrong = "Potwierdź poprawnie nowe hasło.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.MessageInfoWrong = "Podałeś nieprawidłowe aktualne hasło.";
                    return View();
                }

                return RedirectToAction("DashboardIndex", "Dashboard");
            }
        }

        public ActionResult EditAccount()
        {
            DbContextModel db = new DbContextModel();
            
            int UserId = Convert.ToInt32(Session["UserId"]);

            UserBasicModel userToEdit = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));
            if (userToEdit.Skills != null)
            {
                userToEdit.SkillsArray = userToEdit.Skills.Split(',').ToArray();
            }

            var skills = db.Skills.ToList();
            MultiSelectList list = new MultiSelectList(skills, "SkillId", "SkillName");
            ViewBag.Skills = list;

            return View(userToEdit);
        }



        [HttpPost]
        public ActionResult EditAccount(UserBasicModel userAfterEdit, HttpPostedFileBase file)
        {
            int UserId = Convert.ToInt32(Session["UserId"]);
            

            using (DbContextModel db = new DbContextModel())
            {
                if (userAfterEdit.SkillsArray != null)
                {
                    userAfterEdit.Skills = string.Join(",", userAfterEdit.SkillsArray);
                }

                UserBasicModel user = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));

                user.MobileNumber = userAfterEdit.MobileNumber;

                user.DisplayedRole = userAfterEdit.DisplayedRole;

                user.Description = userAfterEdit.Description;
                if (file != null)
                {
                    byte[] data = FileHandler.GetBytesFromFile(file);
                    user.UserPhoto = data;
                }

                user.Skills = userAfterEdit.Skills;
                
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

                if(user.SkillsArray != null)
                {
                    user.SkillsArray = user.Skills.Split(',').ToArray();
                }
                
                var skills = db.Skills.ToList();
                MultiSelectList list = new MultiSelectList(skills, "SkillId", "SkillName");
                ViewBag.Skills = list;
                return View(user);
            }
        }

        public ActionResult ProfileDetails(int id)
        {
            DbContextModel db = new DbContextModel();

            UserBasicModel userDetails = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(id));

            if (userDetails.Skills != null)
            {
                userDetails.SkillsArray = userDetails.Skills.Split(',').ToArray();
                foreach (var skillId in userDetails.SkillsArray)
                {
                    var skillIdInt = Convert.ToInt32(skillId);
                    var skill = db.Skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                    userDetails.SkillsCollection.Add(skill);
                }
            }

            return View(userDetails);
        }

    }
}