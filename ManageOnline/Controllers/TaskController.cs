using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageOnline.Models;

namespace ManageOnline.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task

        public ActionResult AddTask(int projectId)
        {
            TaskModel task = new TaskModel();
            task.ProjectId = projectId;
            return PartialView("_addTask");
        }

        [HttpPost]
        public ActionResult AddTask(TaskModel task)
        {
            int userIdInt = Convert.ToInt32(Session["UserId"]);
            int ProjectId = task.ProjectId;
            using (DbContextModel db = new DbContextModel())
            {

                task.TaskCreationDate = DateTime.Now;
                task.Project = db.Projects.Where(x => x.ProjectId.Equals(ProjectId)).FirstOrDefault();
                task.UserWhoAddTask = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                task.RowNumber = 1;
                task.ColumnNumber = 1;
                db.Tasks.AddOrUpdate(task);
                db.SaveChanges();
            }
            return RedirectToAction("KanbanBoard", "ProjectPanel", new { projectId = ProjectId });
        }

        public ActionResult UpdateTaskPosition(string column1, string column2, string column3)
        {
            var column1Tasks = column1.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var column2Tasks = column2.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var column3Tasks = column3.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            using (DbContextModel db = new DbContextModel())
            {
                int counter1 = 1;
                foreach (var itemId in column1Tasks)
                {
                    TaskModel task = db.Tasks.Where(x => x.TaskId.Equals(itemId)).FirstOrDefault();
                    task.RowNumber = counter1;
                    task.ColumnNumber = 1;
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    counter1++;
                }
                int counter2 = 1;
                foreach (var itemId in column2Tasks)
                {

                    TaskModel task = db.Tasks.Where(x => x.TaskId.Equals(itemId)).FirstOrDefault();
                    task.RowNumber = counter2;
                    task.ColumnNumber = 2;
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    counter2++;
                }
                int counter3 = 1;
                foreach (var itemId in column3Tasks)
                {

                    TaskModel task = db.Tasks.Where(x => x.TaskId.Equals(itemId)).FirstOrDefault();
                    task.RowNumber = counter3
                        ;
                    task.ColumnNumber = 3;
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    counter3++;
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}