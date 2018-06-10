using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageOnline.Controllers
{
    public class MessagesController : Controller
    {
        //GET: Messages

        public ActionResult MessagesInfo(int userId)
        {
            MessageModel message = new MessageModel();
            using (DbContextModel db = new DbContextModel())
            {
                ICollection<MessageModel> messages = db.Messages
                                                    .Include("Receiver")
                                                    .Include("Sender")
                                                    .Where(x => x.Receiver.UserId.Equals(userId)).ToList();

                ViewBag.messageNotSeenCount = messages.Where(x => x.IsSeen == false).Count();
                
                return PartialView("_messagesInfo");
            }
            
        }

        public ActionResult ShowMessageDetails(int messageId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                int userIdInt = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                var message = db.Messages.Include("Receiver")
                                         .Include("Sender")
                                         .Where(x => x.MessageId.Equals(messageId)).FirstOrDefault();

                if (!message.IsSeen && message.Receiver.UserId == userIdInt)
                    message.IsSeen = true;
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_messageDetails", message);
            }

        }
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
                message.IsSeen = false;
                db.Messages.Add(message);
                db.SaveChanges();
            }

            return RedirectToAction("ShowSendedMessages");
        }

        public ActionResult ShowReceivedMessages()
        {
            using (DbContextModel db = new DbContextModel())
            {
                int userIdInt = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                ICollection<MessageModel> messages = db.Messages
                                                    .Include("Receiver")
                                                    .Include("Sender")
                                                    .OrderByDescending(x => x.DateSend)
                                                    .Where(x => x.Receiver.UserId.Equals(userIdInt)).ToList();
                return View(messages);
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
                                                    .OrderByDescending(x => x.DateSend)
                                                    .Where(x => x.Sender.UserId.Equals(senderIdInt)).ToList();
                return View(messages);
            }
        }


    }
}