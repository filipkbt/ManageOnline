using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageOnline.Controllers
{
    public class MessagesController : Controller
    {
        //GET: Messages
        public ActionResult SendMessage(int userId)
        {
            MessageModel message = new MessageModel();
            using (DbContextModel db = new DbContextModel())
            {
                message.Receiver = db.UserAccounts.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
            }
            return PartialView("_sendMessage", message);
        }

        [HttpPost]
        public ActionResult SendMessage(MessageModel message)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var senderIdInt = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                var receiverUserId = message.Receiver.UserId;
                message.Receiver = db.UserAccounts.Where(x => x.UserId.Equals(receiverUserId)).FirstOrDefault();
                message.Sender = db.UserAccounts.Where(x => x.UserId.Equals(senderIdInt)).FirstOrDefault();
                message.DateSend = DateTime.Now;
                db.Messages.Add(message);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    var ble = ex;
                }

            }

            return RedirectToAction("ShowSendedMessages");
        }

        public ActionResult ShowReceivedMessages()
        {
            using (DbContextModel db = new DbContextModel())
            {
                ICollection<MessageModel> messages = db.Messages
                                                    .Include("Receiver")
                                                    .Include("Sender")
                                                    .Where(x => x.Receiver.UserId.Equals(System.Web.HttpContext.Current.Session["UserId"])).ToList();
                return View();
            }
        }


        public ActionResult ShowSendedMessages()
        {
            using (DbContextModel db = new DbContextModel())
            {
                int senderIdInt = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                ICollection<MessageModel> messages = db.Messages
                                                    .Include("Receiver")
                                                    .Include("Sender")
                                                    .Where(x => x.Sender.UserId.Equals(senderIdInt)).ToList();
                return View();
            }
        }


    }
}