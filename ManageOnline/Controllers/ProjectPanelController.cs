using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ManageOnline.Models;

namespace ManageOnline.Controllers
{
    public class ProjectPanelController : Controller
    {
        // GET: ProjectPanel
        public ActionResult ProjectPanel(int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var project = db.Projects
                    .Include("ProjectOwner")
                    .Include("SkillsRequiredToProjectCollection")
                    .Include("ProjectCategory")
                    .Include("Tasks.CurrentWorkerAtTask")
                    .Include("Tasks.UserWhoAddTask")
                    .Include("UsersBelongsToProjectCollection")
                    .Where(x => x.ProjectId.Equals(projectId))
                    .FirstOrDefault();


                var categoriesList = db.Categories.ToList();
                var skills = db.Skills.ToList();

                var projectCategoryId = Convert.ToInt32(project.ProjectCategory.CategoryId);
                project.ProjectCategory = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                if (project.SkillsRequiredToProject != null)
                {
                    project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                    project.SkillsRequiredToProjectCollection = new Collection<SkillsModel>();
                    foreach (var skillId in project.SkillsRequiredToProjectArray)
                    {
                        var skillIdInt = Convert.ToInt32(skillId);
                        var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                        project.SkillsRequiredToProjectCollection.Add(skill);
                    }
                }
                ViewBag.ProjectManagementMethodology = project.ProjectManagementMethodology;
                return View(project);
            }
        }

        public ActionResult KanbanBoard(int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var project = db.Projects
                    .Include("ProjectOwner")
                    .Include("SkillsRequiredToProjectCollection")
                    .Include("ProjectCategory")
                    .Include("Tasks.CurrentWorkerAtTask")
                    .Include("Tasks.UserWhoAddTask")
                    .Include("UsersBelongsToProjectCollection")
                    .Where(x => x.ProjectId.Equals(projectId))
                    .FirstOrDefault();

                ViewBag.CountAllTasks = project.Tasks.Count();
                ViewBag.CountNotStartedTasks = project.Tasks.Where(x=>x.TaskStatus == TaskStatus.NotStarted).Count();
                ViewBag.CountInProgressTasks = project.Tasks.Where(x => x.TaskStatus == TaskStatus.InProgress).Count();
                ViewBag.CountFinishedTasks = project.Tasks.Where(x => x.TaskStatus == TaskStatus.Finished).Count();
                var progressBarNotStartedTasksWidth = (((double)ViewBag.CountNotStartedTasks / (double)ViewBag.CountAllTasks) * 100).ToString() ;
                ViewBag.progressBarNotStartedTasksWidth = progressBarNotStartedTasksWidth.Replace(",", ".");
                var progressBarInProgressTasksWidth = (((double)ViewBag.CountInProgressTasks / (double)ViewBag.CountAllTasks) * 100).ToString() ;
                ViewBag.progressBarInProgressTasksWidth = progressBarInProgressTasksWidth.Replace(",", ".");
                var progressBarFinishedTasksWidth = (((double)ViewBag.CountFinishedTasks / (double)ViewBag.CountAllTasks) * 100).ToString() ;
                ViewBag.progressBarFinishedTasksWidth = progressBarFinishedTasksWidth.Replace(",", ".");
                return View(project);
            }
        }

        public ActionResult ScrumBoard(int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var project = db.Projects
                    .Include("ProjectOwner")
                    .Include("SkillsRequiredToProjectCollection")
                    .Include("ProjectCategory")
                    .Include("Tasks.CurrentWorkerAtTask")
                    .Include("Tasks.UserWhoAddTask")
                    .Include("UsersBelongsToProjectCollection")
                    .Include("ScrumSprints")
                    .Where(x => x.ProjectId.Equals(projectId))
                    .FirstOrDefault();

                project.ScrumSprints = db.ScrumSprints.Where(x => x.Project.ProjectId.Equals(projectId)).OrderBy(x=>x.ScrumSprintNumber).ToList();
                ViewBag.ProjectId = projectId;
                ViewBag.ProjectStatus = project.ProjectStatus;

                return View(project);
            }
        }

        [HttpPost]
        public ActionResult AddScrumSprint(int projectId, int scrumSprintLengthInDays)
        {
            using(DbContextModel db = new DbContextModel())
            {
                ScrumSprintModel scrumSprint = new ScrumSprintModel();
                scrumSprint.Project = db.Projects.Where(x => x.ProjectId == projectId).FirstOrDefault();
                scrumSprint.StartScrumSprintDate = DateTime.Now;
                scrumSprint.FinishScrumSprintDate = DateTime.Now.AddDays(scrumSprintLengthInDays);
                scrumSprint.ScrumSprintLengthInDays = scrumSprintLengthInDays;
                var lastSprint = db.ScrumSprints.Where(x => x.Project.ProjectId == projectId).OrderByDescending(x => x.ScrumSprintNumber).FirstOrDefault();
                int sprintNumber = lastSprint.ScrumSprintNumber;
                if(lastSprint != null)
                {
                    sprintNumber++;
                }
                else
                {
                    sprintNumber = 1;
                }

                scrumSprint.ScrumSprintNumber = sprintNumber;
                db.ScrumSprints.Add(scrumSprint);
                db.SaveChanges();
            }
            return RedirectToAction("ScrumBoard", new { projectId = projectId });
        }
        
        public ActionResult DeleteScrumSprint(int projectId, int scrumSprintId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var scrumSprint = db.ScrumSprints.Where(x => x.ScrumSprintId == scrumSprintId).FirstOrDefault();

                var tasksBelongedToScrumSprint = db.Tasks.Include("ScrumSprintWhereTaskBelong").Where(x => x.ScrumSprintWhereTaskBelong.ScrumSprintId == scrumSprintId).ToList();

                foreach (var task in tasksBelongedToScrumSprint)
                {
                    db.Tasks.Remove(task);
                    db.SaveChanges();
                }

                db.ScrumSprints.Remove(scrumSprint);
                db.SaveChanges();

                return RedirectToAction("ScrumBoard", "ProjectPanel", new { projectId = projectId });
            }
        }

        public ActionResult MarkScrumSprintAsFinished(int projectId, int scrumSprintId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var scrumSprint = db.ScrumSprints.Where(x => x.ScrumSprintId == scrumSprintId).FirstOrDefault();

                scrumSprint.FinishScrumSprintDate = DateTime.Now;
                scrumSprint.IsFinished = true;

                db.Entry(scrumSprint).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("ScrumBoard", "ProjectPanel", new { projectId = projectId });
            }
        }

        public ActionResult ProjectFiles(int projectId)
        {
            using(DbContextModel db = new DbContextModel())
            {
                var project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                ViewBag.ProjectStatus = project.ProjectStatus;
                ViewBag.ProjectId = projectId;
                ViewBag.ProjectManagementMethodology = project.ProjectManagementMethodology;
                var files = db.Files.Include("Project")
                                    .Include("UserWhoAddFile")
                                    .OrderBy(x => x.DateUploadFile)
                                    .Where(x => x.Project.ProjectId.Equals(projectId))
                                    .ToList();
                
                return View(files);
            }
            
        }

        public ActionResult UploadFile(int projectId)
        {
            FileModel file = new FileModel();
            using(DbContextModel db = new DbContextModel())
            {
                file.Project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
            }            

            return PartialView("_uploadFile",file);
        }

        [HttpPost]
        public ActionResult UploadFile(FileModel fileObject, HttpPostedFileBase file)
        {
            int projectId = fileObject.Project.ProjectId;
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
            if (file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string filePath = "~/Files/" + projectId +"/";
                if(!Directory.Exists(filePath))
                    Directory.CreateDirectory(HostingEnvironment.ApplicationPhysicalPath + "/Files/" + projectId + "/");
                string path = Path.Combine(Server.MapPath(filePath), fileName);
                file.SaveAs(path);
                using (DbContextModel db = new DbContextModel())
                {
                    fileObject.FilePath = path;
                    fileObject.DateUploadFile = DateTime.Now;
                    fileObject.Project = db.Projects.Include("ProjectOwner").Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                    fileObject.UserWhoAddFile = db.UserAccounts.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
                    db.Files.Add(fileObject);

                    fileObject.Project.UsersBelongsToProjectArray = fileObject.Project.UsersBelongsToProject.Split(',').ToArray();

                    foreach (var userInProject in fileObject.Project.UsersBelongsToProjectArray)
                    {
                        int userInProjectIdInt = Convert.ToInt32(userInProject);                        
                        var user = db.UserAccounts.Where(x => x.UserId.Equals(userInProjectIdInt)).FirstOrDefault();
                        if(user.UserId != userId)
                        {
                            db.Notifications.Add(new NotificationModel { Project = fileObject.Project, NotificationType = NotificationTypes.DodaniePlikuDoProjektu, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = user, Title="Nowy plik w projekcie", Content = string.Format("Użytkownik {0} przesłał plik {1} do projektu {2}", fileObject.UserWhoAddFile.Username, fileObject.FileName, fileObject.Project.ProjectTitle) });
                        }                        
                    }
                    if (fileObject.Project.ProjectOwner.UserId != userId)
                    {
                        db.Notifications.Add(new NotificationModel { Project = fileObject.Project, NotificationType = NotificationTypes.DodaniePlikuDoProjektu, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = fileObject.Project.ProjectOwner,Title="Nowy plik w projekcie", Content = string.Format("Użytkownik {0} przesłał plik {1} do projektu {2}", fileObject.UserWhoAddFile.Username, fileObject.FileName, fileObject.Project.ProjectTitle) });
                    }                  

                    db.SaveChanges();
                }
            }
            return RedirectToAction("ProjectFiles", new { projectId = projectId });
        }

        public FileResult DownloadFile(int fileId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var file = db.Files.Where(x => x.FileId.Equals(fileId)).First();

                return File(file.FilePath,"application/force-download", Path.GetFileName(file.FilePath));
            }
        }
    }
}