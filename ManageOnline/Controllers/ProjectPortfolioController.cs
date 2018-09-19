using ManageOnline.Infrastructure;
using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageOnline.Controllers
{
    public class ProjectPortfolioController : Controller
    {
        public ActionResult EditPortfolio()
        {
            return View("EditPortfolio");
        }
        // GET: ProjectPortfolio
        public ActionResult AddProjectToPortfolio()
        {
            PortfolioProjectModel portfolioProject = new PortfolioProjectModel();
            
            return PartialView("_addProjectToPortfolio");
        }

        [HttpPost]
        public ActionResult AddProjectToPortfolio(PortfolioProjectModel portfolioProject, HttpPostedFileBase file)
        {
            int userIdInt = Convert.ToInt32(Session["UserId"]);
            using (DbContextModel db = new DbContextModel())
            {
                portfolioProject.EmployeeId = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                if (file != null)
                {
                    byte[] data = FileHandler.GetBytesFromFile(file);
                    portfolioProject.ProjectImage = data;
                }
                db.PortfolioProjects.Add(portfolioProject);
                try
                {
                    db.SaveChanges();
                    ViewBag.MessageAfterEditProfileDetails = "Edycja danych przebiegła pomyślnie.";
                }
                catch(Exception ex)
                {
                    ViewBag.MessageAfterEditProfileDetails = "Edycja danych się nie udała." + ex.Message;
                    return View();
                }
            }
            return RedirectToAction("EditAccount", "Account");
        }

        public ActionResult ProjectsFromPortfolio()
        {
            int userIdInt = Convert.ToInt32(Session["UserId"]);

            using (DbContextModel db = new DbContextModel())
            {
                var ProjectsFromPortfolio = db.PortfolioProjects.Where(x => x.EmployeeId.UserId.Equals(userIdInt)).ToList();

                if(ProjectsFromPortfolio != null)
                {
                    return PartialView("_ProjectsFromPortfolio", ProjectsFromPortfolio);
                }
                else
                {
                    return PartialView("_ProjectsFromPortfolio");
                }
                
            }            
        }

        public ActionResult ProjectsFromPortfolioSelectedUser(int userId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var ProjectsFromPortfolio = db.PortfolioProjects.Where(x => x.EmployeeId.UserId.Equals(userId)).ToList();

                if (ProjectsFromPortfolio != null)
                {
                    return PartialView("_ProjectsFromPortfolioSelectedUser", ProjectsFromPortfolio);
                }
                else
                {
                    return PartialView("_ProjectsFromPortfolioSelectedUser");
                }

            }
        }

    }
}