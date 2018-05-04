using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                var projectDetailsInfo = db.Projects
                    .Include("ProjectOwner")
                    .Include("OffersToProject")
                    .Include("OffersToProject.UserWhoAddOffer")
                    .FirstOrDefault(p => p.ProjectId.Equals(id));

                foreach (var offer in projectDetailsInfo.OffersToProject)
                {

                    offer.WorkersProposedToProjectArray = offer.WorkersProposedToProject.Split(',').ToArray();
                    foreach (var worker in offer.WorkersProposedToProjectArray)
                    {
                        int workerId = Convert.ToInt32(worker);
                        var workerData = db.UserAccounts.Where(x => x.UserId.Equals((workerId))).FirstOrDefault();
                        offer.WorkersProposedToProjectCollection.Add(workerData);
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
                var employees = db.UserAccounts.Where(x => x.Role == Roles.Pracownik).ToList();
                MultiSelectList list = new MultiSelectList(employees, "UserId", "UserName");
                ViewBag.Employees = list;
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
                offerToProject.WorkersProposedToProject = string.Join(",", offerToProject.WorkersProposedToProjectArray);
                db.Projects.Attach(offerToProject.ProjectWhereOfferWasAdded);
                db.OfferToProjectModels.Add(offerToProject);


                db.SaveChanges();
                return View("SuccessfullAddOffer");
            }
        }

        public ActionResult EditOfferToProject(int offerId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var selectedOffer = db.OfferToProjectModels.Where(x => x.OfferToProjectId == offerId).FirstOrDefault();
                selectedOffer.WorkersProposedToProjectArray = selectedOffer.WorkersProposedToProject.Split(',').ToArray();

                var employees = db.UserAccounts.Where(x => x.Role == Roles.Pracownik).ToList();
                MultiSelectList list = new MultiSelectList(employees, "UserId", "UserName");
                ViewBag.Employees = list;

                return View(selectedOffer);
            }

        }

        [HttpPost]
        public ActionResult EditOfferToProject(OfferToProjectModel offerToProject)
        {
            offerToProject.WorkersProposedToProject = string.Join(",", offerToProject.WorkersProposedToProjectArray);
            using (DbContextModel db = new DbContextModel())
            {
                int offerToProjectId = offerToProject.OfferToProjectId;

                OfferToProjectModel oldOffer = db.OfferToProjectModels.FirstOrDefault(x => x.OfferToProjectId.Equals(offerToProjectId));

                oldOffer.Budget = offerToProject.Budget;
                oldOffer.Description = offerToProject.Description;
                oldOffer.EstimatedTimeToFinishProject = offerToProject.EstimatedTimeToFinishProject;
                oldOffer.WorkersProposedToProject = offerToProject.WorkersProposedToProject;

                db.Entry(oldOffer).State = EntityState.Modified;
                db.SaveChanges();
                return View("SuccessfullEditOffer");
            }
        }

        public ActionResult AdmitSelectedProject(int projectId, int offerId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                ProjectModel project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                OfferToProjectModel offer = db.OfferToProjectModels.Where(x => x.OfferToProjectId.Equals(offerId)).FirstOrDefault();
                offer.WorkersProposedToProject += "," + offer.UserWhoAddOffer.UserId;
                project.UsersBelongsToProject = string.Join(",", offer.WorkersProposedToProject);

                project.ProjectStartDate = DateTime.Now;
                project.ProjectStatus = ProjectStatus.InProgress;

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                return View("SuccessfulAdmitProject");
            }
        }

        public ActionResult SuccessfullAddProject()
        {
            return View();
        }

        public ActionResult SuccessfullEditOffer()
        {
            return View();
        }

        public ActionResult ShowYourProjects(int id)
        {
            return View();
        }


        public ActionResult SuccessfullAddOffer()
        {
            return View();
        }


    }
}