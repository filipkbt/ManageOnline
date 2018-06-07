using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    projectOwner.IsRatedAtCurrentProject = CheckIfUserIsRated(projectId, project.ProjectOwner.UserId);

                    project.UsersBelongsToProjectCollection.Add(projectOwner);

                    if (project.IsRequiredManager)
                    {
                        var manager = db.UserAccounts.Where(x => x.UserId.Equals(project.Manager.UserId)).FirstOrDefault();
                        manager.IsRatedAtCurrentProject = CheckIfUserIsRated(projectId, project.Manager.UserId);
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
                        user.IsRatedAtCurrentProject = CheckIfUserIsRated(projectId, userIdInt);
                        project.UsersBelongsToProjectCollection.Add(user);
                    }
                    var projectOwner = db.UserAccounts.Where(x => x.UserId.Equals(project.ProjectOwner.UserId)).FirstOrDefault();
                    projectOwner.IsRatedAtCurrentProject = CheckIfUserIsRated(projectId, project.ProjectOwner.UserId);
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
                        user.IsRatedAtCurrentProject = CheckIfUserIsRated(projectId, userIdInt);
                        project.UsersBelongsToProjectCollection.Add(user);
                    }

                    if (project.IsRequiredManager)
                    {
                        var manager = db.UserAccounts.Where(x => x.UserId.Equals(project.Manager.UserId)).FirstOrDefault();
                        manager.IsRatedAtCurrentProject = CheckIfUserIsRated(projectId, project.Manager.UserId);
                        project.UsersBelongsToProjectCollection.Add(manager);
                    }
                    return View(project);
                }
            }

            return View();
        }

        public async Task<ActionResult> RateUser(int projectId, int userId)
        {
            RateModel rate = new RateModel();
            using (DbContextModel db = new DbContextModel())
            {
                rate.Project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                rate.UserWhoGetRate =  db.UserAccounts.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
            }
             return PartialView("_rateUser", rate);
        }

        [HttpPost]
        public async Task<ActionResult> RateUser(RateModel rate)
        {
            float RatesSum = 0;
            using (DbContextModel db = new DbContextModel())
            {

                rate.Project = db.Projects.Where(x => x.ProjectId.Equals(rate.Project.ProjectId)).FirstOrDefault();
                rate.UserWhoGetRate = db.UserAccounts.Where(x => x.UserId.Equals(rate.UserWhoGetRate.UserId)).FirstOrDefault();

                RatesSum += rate.Communication;
                RatesSum += rate.MeetingTheConditions;
                RatesSum += rate.Professionalism;
                RatesSum += rate.WantToCoworkAgain;
                if (rate.UserWhoGetRate.Role.ToString() == "Pracownik")
                {
                    RatesSum += (float)rate.Punctuality;
                    RatesSum += (float)rate.Quality;
                    RatesSum += (float)rate.Skills;
                    rate.AverageRate = RatesSum / 7;
                }

                else if (rate.UserWhoGetRate.Role.ToString() == "Menadzer")
                {
                    RatesSum += (float)rate.ManageSkills;
                    rate.AverageRate = RatesSum / 5;
                }
                else
                {
                    rate.AverageRate = RatesSum / 4;
                }
                db.Rates.Add(rate);
                db.SaveChanges();
            }
            return RedirectToAction("RateUsersFromProject", new { projectId = rate.Project.ProjectId });
        }

        public async Task<ActionResult> ShowRateDetails(int projectId, int userId)
        {            
            using (DbContextModel db = new DbContextModel())
            {
                var rate = db.Rates.Where(x => x.Project.ProjectId == projectId && x.UserWhoGetRate.UserId == userId).FirstOrDefault();
                rate.Project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                rate.UserWhoGetRate = db.UserAccounts.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
                return PartialView("_rateUserDetails", rate);
            }            
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