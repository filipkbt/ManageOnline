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

        //public ActionResult _addOfferToProject()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult _addOfferToProject(ProjectModel project, OfferToProjectModel offer)
        //{
            
        //    using (DbContextModel db = new DbContextModel())
        //    {
        //        offer.ProjectWhereOfferWasAdded = project;
        //        offer.UserWhoAddOffer = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(project.ProjectOwner.UserId));

        //        db.OfferToProjectModels.Add(offer);

        //        db.SaveChanges();
        //        ViewBag.IsOfferAdded = "Dodałeś poprawnie ofertę. Powodzenia !";
        //        return View();
        //    }
        //}

    }
}