using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageOnline.Controllers
{
    public class CommentsController : Controller
    {
        // GET: Comments
        public ActionResult ShowComments(int taskId, int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var commentsConnectedWithTask = db.Comments.Include("TaskWhereCommentBelong")
                                                           .Include("CommentConnectedWithSelectedComment")
                                                           .Include("UserWhoAddComment")
                                                           .Where(x => x.TaskWhereCommentBelong.TaskId == taskId && x.CommentConnectedWithSelectedComment == null)
                                                           .OrderBy(x => x.DateWhenCommentWasAdded)
                                                           .ToList();

                var commentsConnectedWithSelectedComment = db.Comments.Include("TaskWhereCommentBelong")
                                                           .Include("CommentConnectedWithSelectedComment")
                                                           .Include("UserWhoAddComment")
                                                           .OrderBy(x => x.DateWhenCommentWasAdded)
                                                           .Where(x => x.TaskWhereCommentBelong.TaskId == taskId && x.CommentConnectedWithSelectedComment != null)
                                                           .ToList();

                ViewBag.commentsConnectedWithSelectedComment = commentsConnectedWithSelectedComment;
                ViewBag.TaskId = taskId;
                ViewBag.ProjectId = projectId;
                return PartialView("_showComments", commentsConnectedWithTask);        
            }
        }

        public ActionResult AddCommentToTheTask(int taskId, int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                int userIdInt = Convert.ToInt32(Session["UserId"]);
                CommentModel comment = new CommentModel();
                comment.UserWhoAddComment = db.UserAccounts.Where(x => x.UserId == userIdInt).FirstOrDefault();
                comment.TaskWhereCommentBelong = db.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault();
                comment.ProjectWhereCommentBelong = db.Projects.Where(x => x.ProjectId == projectId).FirstOrDefault();
                return PartialView("_addCommentToTheTask", comment);
            }                
        }

        public ActionResult AddCommentToTheComment(int commentId, int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                int userIdInt = Convert.ToInt32(Session["UserId"]);
                CommentModel comment = new CommentModel();
                comment.UserWhoAddComment = db.UserAccounts.Where(x => x.UserId == userIdInt).FirstOrDefault();
                comment.CommentConnectedWithSelectedComment = db.Comments.Include("TaskWhereCommentBelong").Where(x => x.CommentId == commentId).FirstOrDefault();
                comment.TaskWhereCommentBelong = db.Tasks.Where(x=> x.TaskId == comment.CommentConnectedWithSelectedComment.TaskWhereCommentBelong.TaskId).FirstOrDefault();
                comment.ProjectWhereCommentBelong = db.Projects.Where(x => x.ProjectId == projectId).FirstOrDefault();
                return PartialView("_addCommentToTheComment", comment);
            }
            
        }


            [HttpPost]
        public ActionResult AddCommentToTheTask(CommentModel comment)
        {

            using (DbContextModel db = new DbContextModel())
            {
                int userIdInt = Convert.ToInt32(Session["UserId"]);
                comment.DateWhenCommentWasAdded = DateTime.Now;
                comment.ProjectWhereCommentBelong = db.Projects.Where(x => x.ProjectId == comment.ProjectWhereCommentBelong.ProjectId).FirstOrDefault();
                comment.TaskWhereCommentBelong = db.Tasks.Include("CurrentWorkerAtTask").Include("UserWhoAddTask").Where(x => x.TaskId == comment.TaskWhereCommentBelong.TaskId).FirstOrDefault();
                comment.UserWhoAddComment = db.UserAccounts.Where(x => x.UserId == userIdInt).FirstOrDefault();
                db.Notifications.Add(new NotificationModel { Project = comment.ProjectWhereCommentBelong, NotificationType = NotificationTypes.NowyKomentarzDoZadania, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = comment.TaskWhereCommentBelong.UserWhoAddTask, Title = "Nowy komentarz do twojego zadania", Content = string.Format("Użytkownik ({0}) skomentował twoje zadanie  {1} w projekcie {2}. ", comment.UserWhoAddComment.Username, comment.TaskWhereCommentBelong.TaskName, comment.ProjectWhereCommentBelong.ProjectTitle) });
                if(comment.TaskWhereCommentBelong.UserWhoAddTask != comment.TaskWhereCommentBelong.CurrentWorkerAtTask)
                {
                    db.Notifications.Add(new NotificationModel { Project = comment.ProjectWhereCommentBelong, NotificationType = NotificationTypes.NowyKomentarzDoZadania, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = comment.TaskWhereCommentBelong.CurrentWorkerAtTask, Title = "Nowy komentarz do twojego zadania", Content = string.Format("Użytkownik ({0}) skomentował twoje zadanie  {1} w projekcie {2}. ", comment.UserWhoAddComment.Username, comment.TaskWhereCommentBelong.TaskName, comment.ProjectWhereCommentBelong.ProjectTitle) });
                }               
                db.Comments.Add(comment);
                db.SaveChanges();

                if(comment.ProjectWhereCommentBelong.ProjectManagementMethodology == ManageOnline.Models.ProjectManagementMethodology.Scrum)
                {
                    return RedirectToAction("ScrumBoard", "ProjectPanel", new { projectId = comment.TaskWhereCommentBelong.Project.ProjectId });
                }
                return RedirectToAction("KanbanBoard", "ProjectPanel", new { projectId = comment.TaskWhereCommentBelong.Project.ProjectId });
            }
        }

        [HttpPost]
        public ActionResult AddCommentToTheComment(CommentModel comment)
        {

            using (DbContextModel db = new DbContextModel())
            {
                int userIdInt = Convert.ToInt32(Session["UserId"]);
                comment.DateWhenCommentWasAdded = DateTime.Now;
                comment.ProjectWhereCommentBelong = db.Projects.Where(x => x.ProjectId == comment.ProjectWhereCommentBelong.ProjectId).FirstOrDefault();
                comment.CommentConnectedWithSelectedComment = db.Comments.Include("TaskWhereCommentBelong").Include("UserWhoAddComment").Where(x => x.CommentId == comment.CommentConnectedWithSelectedComment.CommentId).FirstOrDefault();
                comment.TaskWhereCommentBelong = db.Tasks.Where(x => x.TaskId == comment.CommentConnectedWithSelectedComment.TaskWhereCommentBelong.TaskId).FirstOrDefault();               
                comment.UserWhoAddComment = db.UserAccounts.Where(x => x.UserId == userIdInt).FirstOrDefault();
                db.Notifications.Add(new NotificationModel { Project = comment.ProjectWhereCommentBelong, NotificationType = NotificationTypes.NowyKomentarzDoKomentarza, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = comment.CommentConnectedWithSelectedComment.UserWhoAddComment, Title = "Nowy komentarz do twojej wypowiedzi", Content = string.Format("Użytkownik ({0}) skomentował twoją wypowiedź związaną z zadaniem {1} w projekcie {2}. ",comment.UserWhoAddComment.Username,comment.TaskWhereCommentBelong.TaskName, comment.ProjectWhereCommentBelong.ProjectTitle) });
                db.Comments.Add(comment);
                db.SaveChanges();

                if (comment.ProjectWhereCommentBelong.ProjectManagementMethodology == ManageOnline.Models.ProjectManagementMethodology.Scrum)
                {
                    return RedirectToAction("ScrumBoard", "ProjectPanel", new { projectId = comment.TaskWhereCommentBelong.Project.ProjectId });
                }
                return RedirectToAction("KanbanBoard", "ProjectPanel", new { projectId = comment.TaskWhereCommentBelong.Project.ProjectId });
            }
        }

    }
}