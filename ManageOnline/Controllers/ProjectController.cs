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
            return View();
        }

        [HttpPost]
        public ActionResult AddProject(ProjectModel project)
        {
            using (DbContextModel db = new DbContextModel())
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                project.ProjectCreationDate = DateTime.Now;
                project.ProjectOwner = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));

                db.Projects.Add(project);
                db.SaveChanges();
            }

            return View();
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
                    .Include("SkilsRequiredToProject")
                    .FirstOrDefault(p => p.ProjectId.Equals(id));
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
                int UserId = Convert.ToInt32(Session["UserId"]);
                offerToProject.UserWhoAddOffer = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));
                offerToProject.AddOfferDate = DateTime.Now;
                db.UserAccounts.Attach(offerToProject.UserWhoAddOffer);
                db.Projects.Attach(offerToProject.ProjectWhereOfferWasAdded);
                db.OfferToProjectModels.Add(offerToProject);
                db.SaveChanges();
                ViewBag.IsOfferAdded = "Dodałeś poprawnie ofertę. Powodzenia !";
                return View();
            }
        }

    }
}