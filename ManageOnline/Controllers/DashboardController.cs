using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageOnline.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult DashboardIndex()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.UserName = User.Identity.Name;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


    }
}