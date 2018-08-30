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
                var commentsConnectedWithTask = db.Comments.Include("TaskWhereCommentBelong").Where(x => x.TaskWhereCommentBelong.TaskId == taskId).ToList();
                ViewBag.TaskId = taskId;
                ViewBag.ProjectId = projectId;
                return PartialView("_showComments", commentsConnectedWithTask);        
            }
        }

        public ActionResult AddCommentToTheTask(int taskId, int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                CommentModel comment = new CommentModel();
                comment.TaskWhereCommentBelong = db.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault();
                comment.ProjectWhereCommentBelong = db.Projects.Where(x => x.ProjectId == projectId).FirstOrDefault();
                return PartialView("_addCommentToTheTask", comment);
            }                
        }

        public ActionResult AddCommentToTheComment(int commentId, int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                CommentModel comment = new CommentModel();
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
                comment.TaskWhereCommentBelong = db.Tasks.Where(x => x.TaskId == comment.TaskWhereCommentBelong.TaskId).FirstOrDefault();
                comment.UserWhoAddComment = db.UserAccounts.Where(x => x.UserId == userIdInt).FirstOrDefault();
                db.Comments.Add(comment);
                db.SaveChanges();
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
                comment.CommentConnectedWithSelectedComment = db.Comments.Include("TaskWhereCommentBelong").Where(x => x.CommentId == comment.CommentConnectedWithSelectedComment.CommentId).FirstOrDefault();
                comment.TaskWhereCommentBelong = db.Tasks.Where(x => x.TaskId == comment.CommentConnectedWithSelectedComment.TaskWhereCommentBelong.TaskId).FirstOrDefault();
               
                comment.UserWhoAddComment = db.UserAccounts.Where(x => x.UserId == userIdInt).FirstOrDefault();
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("KanbanBoard", "ProjectPanel", new { projectId = comment.TaskWhereCommentBelong.Project.ProjectId });
            }
        }

    }
}