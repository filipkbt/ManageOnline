using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageOnline.Controllers
{
    public class RateController : Controller
    {
        // GET: Rate
        public ActionResult RateUsersFromProject(int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var project = db.Projects.Include("ProjectOwner").Include("Manager").Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                if (System.Web.HttpContext.Current.Session["Role"].ToString() == "Pracownik")
                {
                    var projectOwner = db.UserAccounts.Where(x => x.UserId.Equals(project.ProjectOwner.UserId)).FirstOrDefault();
                    project.UsersBelongsToProjectCollection.Add(projectOwner);

                    if (project.IsRequiredManager)
                    {
                        var manager = db.UserAccounts.Where(x => x.UserId.Equals(project.Manager.UserId)).FirstOrDefault();
                        project.UsersBelongsToProjectCollection.Add(manager);
                    }

                    return View(project);
                }

                else if (System.Web.HttpContext.Current.Session["Role"].ToString() == "Manager")
                {
                    project.UsersBelongsToProjectArray = project.UsersBelongsToProject.Split(',').ToArray();

                    foreach (var userId in project.UsersBelongsToProjectArray)
                    {
                        int userIdInt = Convert.ToInt32(userId);
                        var user = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                        project.UsersBelongsToProjectCollection.Add(user);
                    }
                    var projectOwner = db.UserAccounts.Where(x => x.UserId.Equals(project.ProjectOwner.UserId)).FirstOrDefault();
                    project.UsersBelongsToProjectCollection.Add(projectOwner);
                    return View(project);
                }

                else if (System.Web.HttpContext.Current.Session["Role"].ToString() == "Klient")
                {
                    project.UsersBelongsToProjectArray = project.UsersBelongsToProject.Split(',').ToArray();

                    foreach (var userId in project.UsersBelongsToProjectArray)
                    {
                        int userIdInt = Convert.ToInt32(userId);
                        var user = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                        project.UsersBelongsToProjectCollection.Add(user);
                    }

                    if (project.IsRequiredManager)
                    {
                        var manager = db.UserAccounts.Where(x => x.UserId.Equals(project.Manager.UserId)).FirstOrDefault();
                        project.UsersBelongsToProjectCollection.Add(manager);
                    }
                    return View(project);
                }
            }

            return View();
        }

        private bool CheckIfUserIsRated(int projectId, int userId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var rateConnectedWithUser = db.Rates.Where(x => x.Project.ProjectId == projectId && x.UserWhoGetRate.UserId == userId).FirstOrDefault();
                if (rateConnectedWithUser != null)
                    return true;
                return false;
            }
        }
    }
}