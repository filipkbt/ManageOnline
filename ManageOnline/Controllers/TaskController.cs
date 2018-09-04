using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            task.CurrentWorkerAtTask = new UserBasicModel();
            task.ProjectId = projectId;
            using (DbContextModel db = new DbContextModel())
            {
                ICollection<UserBasicModel> usersBelongsToProject = new Collection<UserBasicModel>();

                var project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                project.UsersBelongsToProjectArray = project.UsersBelongsToProject.Split(',').ToArray();

                foreach (var userId in project.UsersBelongsToProjectArray)
                {
                    int userIdInt = Convert.ToInt32(userId);
                    var user = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                    usersBelongsToProject.Add(user);
                }
                ViewBag.Users = usersBelongsToProject;

            }
            return PartialView("_addTask", task);
        }

        public ActionResult AddTaskToSprint(int projectId, int scrumSprintId)
        {
            TaskModel task = new TaskModel();
            task.CurrentWorkerAtTask = new UserBasicModel();
            task.ProjectId = projectId;
            using (DbContextModel db = new DbContextModel())
            {
                ICollection<UserBasicModel> usersBelongsToProject = new Collection<UserBasicModel>();

                var project = db.Projects.Include("ScrumSprints").Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                project.UsersBelongsToProjectArray = project.UsersBelongsToProject.Split(',').ToArray();

                foreach (var userId in project.UsersBelongsToProjectArray)
                {
                    int userIdInt = Convert.ToInt32(userId);
                    var user = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                    usersBelongsToProject.Add(user);
                }

                task.ScrumSprintWhereTaskBelong = db.ScrumSprints.Where(x => x.ScrumSprintId == scrumSprintId).FirstOrDefault();
                ViewBag.SprintNumber = task.ScrumSprintWhereTaskBelong.ScrumSprintNumber;
                ViewBag.Users = usersBelongsToProject;

                return PartialView("_addTask", task);
            }

        }

        [HttpPost]
        public ActionResult AddTask(TaskModel task)
        {
            int userIdInt = Convert.ToInt32(Session["UserId"]);
            int ProjectId = task.ProjectId;
            if (task.TaskName != null)
            {
                using (DbContextModel db = new DbContextModel())
                {
                    task.TaskCreationDate = DateTime.Now;
                    task.Project = db.Projects.Where(x => x.ProjectId.Equals(ProjectId)).FirstOrDefault();
                    task.UserWhoAddTask = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                    task.CurrentWorkerAtTask = db.UserAccounts.Where(x => x.UserId.Equals(task.CurrentWorkerAtTask.UserId)).FirstOrDefault();
                    var currentSprint = db.ScrumSprints.Where(x => x.ScrumSprintId == task.ScrumSprintWhereTaskBelong.ScrumSprintId).FirstOrDefault();
                    if (task.ScrumSprintWhereTaskBelong != null)
                    {
                        task.ScrumSprintWhereTaskBelong = currentSprint;
                    }
                    task.RowNumber = 1;
                    task.ColumnNumber = 1;

                    db.Tasks.Add(task);
                    db.Notifications.Add(new NotificationModel { Project = task.Project, NotificationType = NotificationTypes.NoweZadanie, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = task.CurrentWorkerAtTask, Title = "Nowe zadanie", Content = string.Format("Użytkownik {0} przypisał Ci zadanie: {1}", task.UserWhoAddTask.Username, task.TaskName) });

                    db.SaveChanges();
                }
            }

            if (task.ScrumSprintWhereTaskBelong != null)
            {
                return RedirectToAction("ScrumBoard", "ProjectPanel", new { projectId = ProjectId });
            }
            return RedirectToAction("KanbanBoard", "ProjectPanel", new { projectId = ProjectId });
        }

        public ActionResult UpdateTaskPosition(string column1, string column2, string column3, int projectId)
        {
            var column1Tasks = column1.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var column2Tasks = column2.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var column3Tasks = column3.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            using (DbContextModel db = new DbContextModel())
            {
                var tasks = db.Projects
                              .Include("Tasks.CurrentWorkerAtTask")
                              .Include("Tasks.UserWhoAddTask")
                              .Include("UsersBelongsToProjectCollection")
                              .Where(x => x.ProjectId.Equals(projectId))
                              .ToList();

                int counter1 = 1;
                foreach (var itemId in column1Tasks)
                {
                    TaskModel task = db.Tasks.Where(x => x.TaskId.Equals(itemId)).FirstOrDefault();
                    task.RowNumber = counter1;
                    task.ColumnNumber = 1;
                    db.Entry(task).State = EntityState.Modified;
                    task.TaskStatus = TaskStatus.NotStarted;
                    db.SaveChanges();
                    counter1++;
                }
                int counter2 = 1;
                foreach (var itemId in column2Tasks)
                {

                    TaskModel task = db.Tasks.Where(x => x.TaskId.Equals(itemId)).FirstOrDefault();
                    task.RowNumber = counter2;
                    task.ColumnNumber = 2;
                    task.TaskStatus = TaskStatus.InProgress;
                    if (task.TaskStartDate == null)
                    {
                        task.TaskStartDate = DateTime.Now;
                    }
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    counter2++;
                }
                int counter3 = 1;
                foreach (var itemId in column3Tasks)
                {
                    TaskModel task = db.Tasks.Where(x => x.TaskId.Equals(itemId)).FirstOrDefault();
                    task.RowNumber = counter3;
                    task.ColumnNumber = 3;
                    task.TaskStatus = TaskStatus.Finished;
                    if (task.TaskFinishDate == null)
                    {
                        task.TaskFinishDate = DateTime.Now;
                    }
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    counter3++;
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTask(int projectId, int taskId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var task = db.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault();
                var project = db.Projects.Where(x => x.ProjectId == projectId).FirstOrDefault();

                db.Tasks.Remove(task);
                db.SaveChanges();

                if (project.ProjectManagementMethodology == ManageOnline.Models.ProjectManagementMethodology.Scrum)
                {
                    return RedirectToAction("ScrumBoard", "ProjectPanel", new { projectId = project.ProjectId });
                }
                return RedirectToAction("KanbanBoard", "ProjectPanel", new { projectId = project.ProjectId });
            }
        }

        public ActionResult ShowTaskDetails(int taskId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var task = db.Tasks.Include("UserWhoAddTask")
                                         .Include("CurrentWorkerAtTask")
                                         .Include("Comments")
                                         .Include("Project")
                                         .Include("Comments.UserWhoAddComment")
                                         .Include("Comments.ProjectWhereCommentBelong")
                                         .Where(x => x.TaskId.Equals(taskId)).FirstOrDefault();

                return PartialView("_showTaskDetails", task);
            }
        }

        public ActionResult EditTask(int taskId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var task = db.Tasks.Include("UserWhoAddTask")
                                         .Include("CurrentWorkerAtTask")
                                         .Include("Project")
                                         .Include("Comments")
                                         .Include("Comments.UserWhoAddComment")
                                         .Include("Comments.ProjectWhereCommentBelong")
                                         .Include("ScrumSprintWhereTaskBelong")
                                         .Where(x => x.TaskId.Equals(taskId)).FirstOrDefault();

                ICollection<UserBasicModel> usersBelongsToProject = new Collection<UserBasicModel>();

                var project = db.Projects.Where(x => x.ProjectId.Equals(task.Project.ProjectId)).FirstOrDefault();
                project.UsersBelongsToProjectArray = project.UsersBelongsToProject.Split(',').ToArray();

                foreach (var userId in project.UsersBelongsToProjectArray)
                {
                    int userIdInt = Convert.ToInt32(userId);
                    var user = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                    usersBelongsToProject.Add(user);
                }

                ViewBag.Users = usersBelongsToProject;

                return PartialView("_editTask", task);
            }
        }

        [HttpPost]
        public ActionResult EditTask(TaskModel task)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var taskToEdit = db.Tasks.Include("UserWhoAddTask")
                                         .Include("CurrentWorkerAtTask")
                                         .Include("Project")
                                         .Include("Comments")
                                         .Include("Comments.UserWhoAddComment")
                                         .Include("Comments.ProjectWhereCommentBelong")
                                         .Where(x => x.TaskId.Equals(task.TaskId)).FirstOrDefault();

                taskToEdit.TaskName = task.TaskName;
                taskToEdit.TaskDescription = task.TaskDescription;
                taskToEdit.CurrentWorkerAtTask = db.UserAccounts.Where(x => x.UserId == task.CurrentWorkerAtTask.UserId).FirstOrDefault();
                taskToEdit.TaskStatus = task.TaskStatus;
                if (task.TaskStatus == ManageOnline.Models.TaskStatus.InProgress)
                    taskToEdit.TaskStartDate = DateTime.Now;
                else if (task.TaskStatus == ManageOnline.Models.TaskStatus.Finished)
                    taskToEdit.TaskFinishDate = DateTime.Now;

                db.Entry(taskToEdit).State = EntityState.Modified;
                db.SaveChanges();

                if (taskToEdit.Project.ProjectManagementMethodology == ManageOnline.Models.ProjectManagementMethodology.Scrum)
                {
                    return RedirectToAction("ScrumBoard", "ProjectPanel", new { projectId = taskToEdit.Project.ProjectId });
                }
                return RedirectToAction("KanbanBoard", "ProjectPanel", new { projectId = taskToEdit.Project.ProjectId });
            }
        }
    }
}