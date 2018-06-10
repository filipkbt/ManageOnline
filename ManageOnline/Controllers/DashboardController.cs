using ManageOnline.Models;
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
            using (DbContextModel db = new DbContextModel())
            {
                var userId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                var notifications = db.Notifications.Include("NotificationReceiver").OrderByDescending(x=>x.DateSend).Where(x => x.NotificationReceiver.UserId.Equals(userId)).ToList();

                return View(notifications);
            }
        }
    }
}