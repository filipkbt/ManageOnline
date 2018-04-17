using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageOnline.Models;
using ManageOnline.ViewModels;

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
                //var projectDetailsInfo = new ProjectUserViewModel();

                var projectDetailsInfo = db.Projects.Include("ProjectOwner").FirstOrDefault(p => p.ProjectId.Equals(id));
                //var projectOwnerId = project.ProjectOwner;
                //projectDetailsInfo.ProjectOwner = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(projectOwnerId));

                //var projectContext = db.Projects.FirstOrDefault(p => p.ProjectId.Equals(id));
                // projectContext.ProjectOwner = db.UserAccounts.FirstOrDefault(u => )
                return View(projectDetailsInfo);
            }

           
            
            
        }
    }
}