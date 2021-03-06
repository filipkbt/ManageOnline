﻿using ManageOnline.Models;
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

            return RedirectToAction("ShowSendedMessages","Messages",null);
        }

        public ActionResult SendMessagesToAllUsers()
        {
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
            MessageModel message = new MessageModel();
            using (DbContextModel db = new DbContextModel())
            {
                message.Sender = db.UserAccounts.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
            }
            return PartialView("_sendMessageToAllUsers", message);
        }

        [HttpPost]
        public ActionResult SendMessageToAllUsers(MessageModel message)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var senderIdInt = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                var users = db.UserAccounts.Where(x => x.Role != Roles.Admin).ToList();
                var admin = db.UserAccounts.Where(x => x.Role == Roles.Admin).FirstOrDefault();
                foreach(var user in users)
                {
                    message.Receiver = db.UserAccounts.Where(x => x.UserId.Equals(user.UserId)).FirstOrDefault();
                    message.Sender = admin;
                    message.DateSend = DateTime.Now;
                    message.IsSeen = false;
                    message.Content = "";
                    db.Messages.Add(message);
                    db.SaveChanges();
                }
            }

            return View("~/Views/Admin/AdminDashboard.cshtml");
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

        public ActionResult ShowReceivedMessagesAdmin()
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

        public ActionResult ShowSendedMessagesAdmin()
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