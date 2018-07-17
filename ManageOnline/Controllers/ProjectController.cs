using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ManageOnline.Models;

namespace ManageOnline.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Project()
        {
            return View();
        }

        public ActionResult AddProject()
        {
            using (DbContextModel db = new DbContextModel())
            {
                var skills = db.Skills.ToList();
                MultiSelectList listSkills = new MultiSelectList(skills, "SkillId", "SkillName");
                ViewBag.Skills = listSkills;

                var categories = db.Categories.ToList();
                SelectList list = new SelectList(categories, "CategoryId", "CategoryName");
                ViewBag.Categories = list;

                return View();
            }
        }

        [HttpPost]
        public ActionResult AddProject(ProjectModel project)
        {
            using (DbContextModel db = new DbContextModel())
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                project.ProjectCreationDate = DateTime.Now;
                project.ProjectOwner = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));
                project.ProjectStatus = ProjectStatus.WaitingForOffers;

                if (project.SkillsRequiredToProjectArray != null)
                {
                    project.SkillsRequiredToProject = string.Join(",", project.SkillsRequiredToProjectArray);
                }

                db.Projects.Add(project);
                db.SaveChanges();

                var skills = db.Skills.ToList();
                MultiSelectList listSkills = new MultiSelectList(skills, "SkillId", "SkillName");
                ViewBag.Skills = listSkills;

                var categories = db.Categories.ToList();
                SelectList list = new SelectList(categories, "CategoryId", "CategoryName");
                ViewBag.Categories = list;
            }

            return View("SuccessfullAddProject");
        }

        public ActionResult SearchProjects()
        {
            using (DbContextModel db = new DbContextModel())
            {
                var dataContext = db.Projects
                    .Include("ProjectOwner")
                    .Include("SkillsRequiredToProjectCollection")
                    .Include("CategoriesModel")
                    .OrderByDescending(x=>x.ProjectCreationDate)
                    .ToList();

                var skills = db.Skills.ToList();
                MultiSelectList skillsList = new MultiSelectList(skills, "SkillId", "SkillName");

                if ((TempData["Skills"] as MultiSelectList) != null)
                {
                    var selectedSkillsSelectList = TempData["Skills"] as MultiSelectList;
                    var selectedSkills = selectedSkillsSelectList.Items;
                }

                TempData["Skills"] = skillsList;

                var categoriesList = db.Categories.ToList();
                foreach (var project in dataContext)
                {
                    var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                    project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();


                    if (project.SkillsRequiredToProject != null)
                    {
                        project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                        foreach (var skillId in project.SkillsRequiredToProjectArray)
                        {
                            var skillIdInt = Convert.ToInt32(skillId);
                            var skill = db.Skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                            project.SkillsRequiredToProjectCollection.Add(skill);
                        }
                    }
                }
                return View(dataContext);
            }
        }

        [HttpPost]
        public ActionResult SearchProjects(FormCollection form)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var dataContext = db.Projects
                    .Include("ProjectOwner")
                    .Include("SkillsRequiredToProjectCollection")
                    .Include("CategoriesModel")
                    .OrderByDescending(x => x.ProjectCreationDate)
                    .ToList();

                var skills = db.Skills.ToList();
                MultiSelectList skillsList = new MultiSelectList(skills, "SkillId", "SkillName");

                TempData["Skills"] = skillsList;

                var categoriesList = db.Categories.ToList();
                foreach (var project in dataContext)
                {
                    var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                    project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();

                    if (project.SkillsRequiredToProject != null)
                    {
                        project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                        foreach (var skillId in project.SkillsRequiredToProjectArray)
                        {
                            var skillIdInt = Convert.ToInt32(skillId);
                            var skill = db.Skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                            project.SkillsRequiredToProjectCollection.Add(skill);
                        }
                    }

                }
                if (form["Skills"] != null)
                {
                    var selectedSkills = form["Skills"];
                    var selectedSkillsArray = selectedSkills.Split(',').ToArray();

                    var filteredDataContext = from p in dataContext
                                              where selectedSkillsArray.Any(val => p.SkillsRequiredToProject.Contains(val))
                                              select p;

                    return View(filteredDataContext);
                }
                return View(dataContext);
            }
        }

        public ActionResult ProjectDetails(int id)
        {
            using (DbContextModel db = new DbContextModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var projectDetailsInfo = db.Projects
                    .Include("ProjectOwner")
                    .Include("OffersToProject")
                    .Include("OffersToProject.UserWhoAddOffer")
                    .Include("Manager")
                    .Include("OffersToProject.WorkerProposedToProject")
                    .FirstOrDefault(p => p.ProjectId.Equals(id));

                var categoriesList = db.Categories.ToList();
                var skills = db.Skills.ToList();

                var projectCategoryId = Convert.ToInt32(projectDetailsInfo.ProjectCategory);
                projectDetailsInfo.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                if (projectDetailsInfo.SkillsRequiredToProject != null)
                {
                    projectDetailsInfo.SkillsRequiredToProjectArray = projectDetailsInfo.SkillsRequiredToProject.Split(',').ToArray();
                    projectDetailsInfo.SkillsRequiredToProjectCollection = new Collection<SkillsModel>();
                    foreach (var skillId in projectDetailsInfo.SkillsRequiredToProjectArray)
                    {
                        var skillIdInt = Convert.ToInt32(skillId);
                        var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                        projectDetailsInfo.SkillsRequiredToProjectCollection.Add(skill);
                    }
                }

                return View(projectDetailsInfo);
            }
        }

        public ActionResult AddOfferToProject(int projectId)
        {
            OfferToProjectModel offerToProject = new OfferToProjectModel();
            using (DbContextModel db = new DbContextModel())
            {
                offerToProject.ProjectWhereOfferWasAdded = db.Projects.FirstOrDefault(p => p.ProjectId.Equals(projectId));
                return View(offerToProject);
            }
        }

        [HttpPost]
        public ActionResult AddOfferToProject(OfferToProjectModel offerToProject)
        {
            using (DbContextModel db = new DbContextModel())
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                offerToProject.UserWhoAddOffer = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(userId));
                offerToProject.AddOfferDate = DateTime.Now;
                offerToProject.WorkerProposedToProject = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(userId));
                db.Projects.Attach(offerToProject.ProjectWhereOfferWasAdded);
                db.OfferToProjectModels.Add(offerToProject);
                var project = db.Projects.Include("ProjectOwner").Where(x => x.ProjectId.Equals(offerToProject.ProjectWhereOfferWasAdded.ProjectId)).FirstOrDefault();
                db.Notifications.Add(new NotificationModel {Project = project, NotificationType = NotificationTypes.NowaOfertaRealizacjiProjektu, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = project.ProjectOwner,Title = "Nowa oferta realizacji projektu", Content = string.Format("W twoim projekcie {0} została dodana nowa oferta realizacji przez użytkownika {1}.", project.ProjectTitle, offerToProject.UserWhoAddOffer.Username) });
                db.SaveChanges();
                return View("SuccessfullAddOffer");
            }
        }

        public ActionResult EditOfferToProject(int offerId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var selectedOffer = db.OfferToProjectModels.Where(x => x.OfferToProjectId == offerId).FirstOrDefault();

                return View(selectedOffer);
            }
        }

        [HttpPost]
        public ActionResult EditOfferToProject(OfferToProjectModel offerToProject)
        {
            using (DbContextModel db = new DbContextModel())
            {
                db.Configuration.LazyLoadingEnabled = false;

                int offerToProjectId = offerToProject.OfferToProjectId;

                OfferToProjectModel oldOffer = db.OfferToProjectModels.FirstOrDefault(x => x.OfferToProjectId.Equals(offerToProjectId));

                oldOffer.Budget = offerToProject.Budget;
                oldOffer.Description = offerToProject.Description;
                oldOffer.EstimatedTimeToFinishProject = offerToProject.EstimatedTimeToFinishProject;

                db.Entry(oldOffer).State = EntityState.Modified;
                db.SaveChanges();
                return View("SuccessfullEditOffer");
            }
        }

        public ActionResult AdmitSelectedProject(int projectId, int offerId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                ProjectModel project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                OfferToProjectModel offer = db.OfferToProjectModels.Where(x => x.OfferToProjectId.Equals(offerId)).Include("WorkerProposedToProject").FirstOrDefault();
                if (project.UsersBelongsToProject == null)
                {
                    project.UsersBelongsToProject = offer.WorkerProposedToProject.UserId.ToString();
                }
                else
                {
                    project.UsersBelongsToProject += "," + offer.WorkerProposedToProject.UserId.ToString();
                }

                db.Notifications.Add(new NotificationModel { Project = project, NotificationType = NotificationTypes.WybranieOfertyRealizacjiProjektu, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = offer.WorkerProposedToProject, Title= "Zaakceptowanie oferty realizacji projektu", Content = string.Format("Twoja oferta realizacji projektu została wybrana w projekcie {0}.", project.ProjectTitle) });
                
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                return View("SuccessfulAdmitProject");
            }
        }

        public ActionResult StartProject(int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                ProjectModel project = db.Projects.Include("ProjectOwner").Include("Manager").Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                project.ProjectStartDate = DateTime.Now;
                project.ProjectStatus = ProjectStatus.InProgress;

                if (project.Manager != null)
                {
                    project.UsersBelongsToProject += string.Format(",{0}",project.Manager.UserId);
                }

                project.UsersBelongsToProjectArray = project.UsersBelongsToProject.Split(',').ToArray();

                foreach (var userId in project.UsersBelongsToProjectArray)
                {
                    int userIdInt = Convert.ToInt32(userId);
                    var user = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                    user.ProjectsInProgress++;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                var ProjectOwner = db.UserAccounts.Where(x => x.UserId.Equals(project.ProjectOwner.UserId)).FirstOrDefault();
                ProjectOwner.ProjectsInProgress++;
                db.Entry(ProjectOwner).State = EntityState.Modified;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                return View("StartProject");
            }
        }
        public ActionResult ShowYourProjects()
        {
            return View();
        }

        public ActionResult ShowProjectsWaitingForOffers()
        {
            string userId = System.Web.HttpContext.Current.Session["UserId"].ToString();
            using (DbContextModel db = new DbContextModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var categoriesList = db.Categories.ToList();
                ICollection<ProjectModel> projects = db.Projects.Include("ProjectOwner")
                                                                .Include("CategoriesModel")
                                                                .Include("SkillsRequiredToProjectCollection")
                                                                .Where(x => x.ProjectStatus == ProjectStatus.WaitingForOffers).ToList();
                ICollection<OfferToProjectModel> offersCollection = db.OfferToProjectModels.Include("UserWhoAddOffer").ToList();
                ICollection<SkillsModel> skills = db.Skills.ToList();
                if (System.Web.HttpContext.Current.Session["Role"].ToString() == Roles.Pracownik.ToString())
                {
                    var filteredProjectsWithOffers = from project in projects
                                                     join offer in offersCollection on project.ProjectId equals offer.ProjectId
                                                     where offer.UserWhoAddOffer.UserId.ToString().Equals(userId)
                                                     select project;

                    foreach (var project in filteredProjectsWithOffers)
                    {
                        var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                        project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                        if (project.SkillsRequiredToProject != null)
                        {
                            project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                            foreach (var skillId in project.SkillsRequiredToProjectArray)
                            {
                                var skillIdInt = Convert.ToInt32(skillId);
                                var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                                project.SkillsRequiredToProjectCollection.Add(skill);
                            }
                        }
                    }

                    return View(filteredProjectsWithOffers);
                }
                if (System.Web.HttpContext.Current.Session["Role"].ToString() == Roles.Manager.ToString())
                {
                    var filteredProjectsWithOffers = from project in projects
                                                     join offer in offersCollection on project.ProjectId equals offer.ProjectId                                            
                                                     where offer.UserWhoAddOffer.UserId.ToString().Equals(userId)
                                                     select project;

                    foreach (var project in filteredProjectsWithOffers)
                    {
                        var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                        project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                        if (project.SkillsRequiredToProject != null)
                        {
                            project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                            foreach (var skillId in project.SkillsRequiredToProjectArray)
                            {
                                var skillIdInt = Convert.ToInt32(skillId);
                                var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                                project.SkillsRequiredToProjectCollection.Add(skill);
                            }
                        }
                    }
                    return View(filteredProjectsWithOffers);
                }

                if (System.Web.HttpContext.Current.Session["Role"].ToString() == Roles.Klient.ToString())
                {
                    var filteredProjects = from project in projects
                                           where project.ProjectOwner.UserId.ToString().Equals(userId)
                                           select project;

                    foreach (var project in filteredProjects)
                    {
                        var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                        project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                        if (project.SkillsRequiredToProject != null)
                        {
                            project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                            foreach (var skillId in project.SkillsRequiredToProjectArray)
                            {
                                var skillIdInt = Convert.ToInt32(skillId);
                                var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                                project.SkillsRequiredToProjectCollection.Add(skill);
                            }
                        }
                    }
                    return View(filteredProjects);
                }
                return View();
            }
        }

        public ActionResult ShowProjectsInProgress()
        {
            string userId = System.Web.HttpContext.Current.Session["UserId"].ToString();
            using (DbContextModel db = new DbContextModel())
            {
                var dataContext = db.Projects
                    .Include("ProjectOwner")
                    .Include("SkillsRequiredToProjectCollection")
                    .Include("CategoriesModel")
                    .Include("OffersToProject")
                    .Where(x => x.ProjectStatus == ProjectStatus.InProgress)
                    .ToList();
                var categoriesList = db.Categories.ToList();
                var skills = db.Skills.ToList();


                if (System.Web.HttpContext.Current.Session["Role"].ToString() == Roles.Pracownik.ToString() ||
                    System.Web.HttpContext.Current.Session["Role"].ToString() == Roles.Manager.ToString())
                {
                    var filteredDataContext = dataContext.Where(x => x.UsersBelongsToProject.Contains(userId)).ToList();

                    foreach (var project in filteredDataContext)
                    {
                        var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                        project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                        if (project.SkillsRequiredToProject != null)
                        {
                            project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                            foreach (var skillId in project.SkillsRequiredToProjectArray)
                            {
                                var skillIdInt = Convert.ToInt32(skillId);
                                var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                                project.SkillsRequiredToProjectCollection.Add(skill);
                            }
                        }
                    }

                    return View(filteredDataContext);
                }
                else if (System.Web.HttpContext.Current.Session["Role"].ToString() == Roles.Klient.ToString())
                {
                    var filteredDataContext = dataContext.Where(x => x.ProjectOwner.UserId.ToString().Equals(userId)).ToList();


                    foreach (var project in filteredDataContext)
                    {
                        var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                        project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                        if (project.SkillsRequiredToProject != null)
                        {
                            project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                            foreach (var skillId in project.SkillsRequiredToProjectArray)
                            {
                                var skillIdInt = Convert.ToInt32(skillId);
                                var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                                project.SkillsRequiredToProjectCollection.Add(skill);
                            }
                        }
                    }

                    return View(filteredDataContext);
                }


                return View();
            }
        }

        public ActionResult ShowProjectsFinished()
        {
            string userId = System.Web.HttpContext.Current.Session["UserId"].ToString();
            using (DbContextModel db = new DbContextModel())
            {
                var dataContext = db.Projects
                    .Include("ProjectOwner")
                    .Include("SkillsRequiredToProjectCollection")
                    .Include("CategoriesModel")
                    .Include("OffersToProject")
                    .Where(x => x.ProjectStatus == ProjectStatus.Finished)
                    .ToList();

                var categoriesList = db.Categories.ToList();
                var skills = db.Skills.ToList();

                if (System.Web.HttpContext.Current.Session["Role"].ToString() == Roles.Pracownik.ToString() ||
                    System.Web.HttpContext.Current.Session["Role"].ToString() == Roles.Manager.ToString())
                {
                    var filteredDataContext = dataContext.Where(x => x.UsersBelongsToProject.Contains(userId)).ToList();

                    foreach (var project in filteredDataContext)
                    {
                        var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                        project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                        if (project.SkillsRequiredToProject != null)
                        {
                            project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                            foreach (var skillId in project.SkillsRequiredToProjectArray)
                            {
                                var skillIdInt = Convert.ToInt32(skillId);
                                var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                                project.SkillsRequiredToProjectCollection.Add(skill);
                            }
                        }
                    }

                    return View(filteredDataContext);
                }
                else if (System.Web.HttpContext.Current.Session["Role"].ToString() == Roles.Klient.ToString())
                {
                    var filteredDataContext = dataContext.Where(x => x.ProjectOwner.UserId.ToString().Equals(userId)).ToList();

                    foreach (var project in filteredDataContext)
                    {
                        var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                        project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                        if (project.SkillsRequiredToProject != null)
                        {
                            project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                            foreach (var skillId in project.SkillsRequiredToProjectArray)
                            {
                                var skillIdInt = Convert.ToInt32(skillId);
                                var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                                project.SkillsRequiredToProjectCollection.Add(skill);
                            }
                        }
                    }

                    return View(filteredDataContext);
                }

                return View();
            }
        }


        public ActionResult SetProjectAsFinished(int projectId)
        {
            ViewBag.ProjectId = projectId;
            using (DbContextModel db = new DbContextModel())
            {
                ProjectModel project = db.Projects.Include("ProjectOwner").Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                project.ProjectFinishDate = DateTime.Now;
                project.ProjectStatus = ProjectStatus.Finished;

                db.Entry(project).State = EntityState.Modified;

                project.UsersBelongsToProjectArray = project.UsersBelongsToProject.Split(',').ToArray();

                foreach (var userId in project.UsersBelongsToProjectArray)
                {
                    int userIdInt = Convert.ToInt32(userId);
                    var user = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                    user.ProjectsInProgress--;
                    user.FinishedProjects++;
                    db.Entry(user).State = EntityState.Modified;
                    db.Notifications.Add(new NotificationModel { Project = project, NotificationType = NotificationTypes.ZakonczenieProjektu, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = user, Title="Zakończenie projektu", Content = string.Format("Projekt {0} został zakończony. Możesz teraz ocenić inne osoby, które brały udział w projekcie.", project.ProjectTitle) });
                    db.SaveChanges();
                }
                project.ProjectOwner.ProjectsInProgress--;
                project.ProjectOwner.FinishedProjects++;
                db.Entry(project.ProjectOwner).State = EntityState.Modified;
                db.Entry(project).State = EntityState.Modified;
                db.Notifications.Add(new NotificationModel { Project = project, NotificationType = NotificationTypes.ZakonczenieProjektu, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = project.ProjectOwner, Title="Zakończenie projektu", Content = string.Format("Projekt {0} został zakończony. Możesz teraz ocenić inne osoby, które brały udział w projekcie.", project.ProjectTitle) });
                db.SaveChanges();
            }
            return View();
            
        }

        public ActionResult SendInvitationToProject(int userId)
        {
            int userWhoSendInvitationId = Convert.ToInt32(Session["UserId"]);
            NotificationModel InvitationNotification = new NotificationModel();
            using (DbContextModel db = new DbContextModel())
            {

                var projectsFromUser = db.Projects
                        .Include("ProjectOwner")
                        .Include("OffersToProject")
                        .Include("OffersToProject.UserWhoAddOffer")
                        .Include("OffersToProject.WorkerProposedToProject")
                        .Where(x => x.ProjectStatus == ProjectStatus.WaitingForOffers && x.ProjectOwner.UserId == userWhoSendInvitationId && x.OffersToProject.Any(item => item.UserWhoAddOffer.UserId.ToString() == userId.ToString()) == false)
                        .ToList();

                InvitationNotification.NotificationReceiver = db.UserAccounts.Where(x => x.UserId.Equals(userId)).FirstOrDefault();

                ViewBag.projectsFromUser = projectsFromUser;
                return PartialView("_sendInvitationToProject", InvitationNotification);
            }
        }

        [HttpPost]
        public ActionResult SendInvitationToProject(NotificationModel notificationInvitationToProject)
        {
            int userWhoSendInvitationId = Convert.ToInt32(Session["UserId"]);
            using (DbContextModel db = new DbContextModel())
            {
                var projectsFromUser = db.Projects
                        .Include("ProjectOwner")
                        .Include("OffersToProject")
                        .Include("OffersToProject.UserWhoAddOffer")
                        .Include("OffersToProject.WorkerProposedToProject")
                        .Where(x => x.ProjectStatus == ProjectStatus.WaitingForOffers && x.ProjectOwner.UserId == userWhoSendInvitationId && x.OffersToProject.Any(item => item.UserWhoAddOffer.UserId.ToString() == notificationInvitationToProject.NotificationReceiver.UserId.ToString()) == false)
                        .ToList();
                ViewBag.projectsFromUser = projectsFromUser;

                var userWhoSendInvitation = db.UserAccounts.Where(x => x.UserId.Equals(userWhoSendInvitationId)).FirstOrDefault();
                notificationInvitationToProject.NotificationReceiver = db.UserAccounts.Where(x => x.UserId.Equals(notificationInvitationToProject.NotificationReceiver.UserId)).FirstOrDefault();
                notificationInvitationToProject.Project = db.Projects.Where(x => x.ProjectId.Equals(notificationInvitationToProject.Project.ProjectId)).FirstOrDefault();
                notificationInvitationToProject.IsSeen = false;
                notificationInvitationToProject.DateSend = DateTime.Now;
                notificationInvitationToProject.NotificationType = NotificationTypes.ZaproszenieDoProjektu;
                notificationInvitationToProject.Title = "Zaproszenie do projektu";
                notificationInvitationToProject.Content = string.Format("Użytkownik {0} przesłał Ci zaproszenie do złożenia oferty w projekcie {1}.", userWhoSendInvitation.Username, notificationInvitationToProject.Project.ProjectTitle);
                db.Notifications.Add(notificationInvitationToProject);
                db.SaveChanges();
                TempData["SuccessfulSendInvitation"] = "Zaproszenie do projektu zostało przesłane.";
                return RedirectToAction("ProfileDetails","Account", new { id = notificationInvitationToProject.NotificationReceiver.UserId });
            }
            
        }

        public ActionResult ConnectProjectWithManager(int projectId)
        {
            int managerId = Convert.ToInt32(Session["UserId"]);

            using(DbContextModel db = new DbContextModel())
            {
                var project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                var manager = db.UserAccounts.Where(x => x.UserId.Equals(managerId)).FirstOrDefault();
                project.Manager = manager;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ProjectDetails", "Project", new { id = projectId });
        }
        public ActionResult SuccessfullAddProject()
        {
            return View();
        }

        public ActionResult SuccessfullEditOffer()
        {
            return View();
        }

        public ActionResult SuccessfullAddOffer()
        {
            return View();
        }
    }
}