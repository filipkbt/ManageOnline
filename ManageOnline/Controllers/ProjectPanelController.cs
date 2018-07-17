﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    .Include("CategoriesModel")
                    .Include("Tasks.CurrentWorkerAtTask")
                    .Include("Tasks.UserWhoAddTask")
                    .Include("UsersBelongsToProjectCollection")
                    .Where(x => x.ProjectId.Equals(projectId))
                    .FirstOrDefault();


                var categoriesList = db.Categories.ToList();
                var skills = db.Skills.ToList();

                var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
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
                    .Include("CategoriesModel")
                    .Include("Tasks.CurrentWorkerAtTask")
                    .Include("Tasks.UserWhoAddTask")
                    .Include("UsersBelongsToProjectCollection")
                    .Where(x => x.ProjectId.Equals(projectId))
                    .FirstOrDefault();

                return View(project);
            }
        }

        public ActionResult ProjectFiles(int projectId)
        {
            using(DbContextModel db = new DbContextModel())
            {
                var project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                ViewBag.ProjectStatus = project.ProjectStatus;
                ViewBag.ProjectId = projectId;
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