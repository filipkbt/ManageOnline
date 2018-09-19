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
                var notifications = db.Notifications.Include("NotificationReceiver").Include("Project").OrderByDescending(x=>x.DateSend).Where(x => x.NotificationReceiver.UserId.Equals(userId)).ToList();
                var NotificationsNotSeen = db.Notifications.Include("NotificationReceiver").OrderByDescending(x => x.DateSend).Where(x => x.NotificationReceiver.UserId == userId && !x.IsSeen).ToList();
                foreach(var notification in NotificationsNotSeen)
                {
                    notification.IsSeen = true;
                }
                db.SaveChanges();
                return View(notifications);
            }
        }

        public ActionResult NotificationsInfo()
        {
            using (DbContextModel db = new DbContextModel())
            {
                var userId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);

                ViewBag.NotificationsNotSeenCount = db.Notifications.Include("NotificationReceiver").OrderByDescending(x => x.DateSend).Where(x => x.NotificationReceiver.UserId == userId && !x.IsSeen).Count();
                return PartialView("_notificationsInfo");
            }
        }

        public ActionResult AdminDashboard()
        {
            return View();
        }
    }
}