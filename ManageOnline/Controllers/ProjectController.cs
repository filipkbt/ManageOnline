using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

                db.Projects.Add(project);
                db.SaveChanges();

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
                var dataContext = db.Projects;
                return View(db.Projects.ToList());
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