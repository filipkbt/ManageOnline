using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            double RatesSum = 0;
            using (DbContextModel db = new DbContextModel())
            {
                int userWhoAddRateIdInt = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                rate.Project = db.Projects.Where(x => x.ProjectId.Equals(rate.Project.ProjectId)).FirstOrDefault();
                rate.UserWhoGetRate = db.UserAccounts.Where(x => x.UserId.Equals(rate.UserWhoGetRate.UserId)).FirstOrDefault();
                rate.UserWhoAddRate = db.UserAccounts.Where(x => x.UserId.Equals(userWhoAddRateIdInt)).FirstOrDefault();

                RatesSum += (double)rate.Communication;
                RatesSum += (double)rate.MeetingTheConditions;
                RatesSum += (double)rate.Professionalism;
                RatesSum += (double)rate.WantToCoworkAgain;
                if (rate.UserWhoGetRate.Role.ToString() == "Pracownik")
                {
                    RatesSum += (double)rate.Punctuality;
                    RatesSum += (double)rate.Quality;
                    RatesSum += (double)rate.Skills;
                    string roundedAverageRate = string.Format("{0:0.00}", RatesSum / 7);
                   rate.AverageRate = Convert.ToDouble(roundedAverageRate);
                }

                else if (rate.UserWhoGetRate.Role.ToString() == "Menadzer")
                {
                    RatesSum += (double)rate.ManageSkills;
                    string roundedAverageRate = string.Format("{0:0.00}", RatesSum / 5);
                    rate.AverageRate = Convert.ToDouble(roundedAverageRate);
                }
                else
                {
                    string roundedAverageRate = string.Format("{0:0.00}", RatesSum / 4);
                    rate.AverageRate = Convert.ToDouble(roundedAverageRate);
                }
                var userRates = db.Rates.Where(x => x.UserWhoGetRate.UserId.Equals(x.UserWhoGetRate.UserId)).ToList();
                double oldRatesSum = 0;
                foreach(var oldRate in userRates)
                {
                    oldRatesSum += oldRate.AverageRate;
                }
                oldRatesSum += rate.AverageRate;
                double oldRatesCount = Convert.ToDouble(userRates.Count());
                oldRatesCount++;
                string roundedAverageRateOverall = string.Format("{0:0.00}", oldRatesSum / oldRatesCount);
                rate.UserWhoGetRate.AverageRate = Convert.ToDouble(roundedAverageRateOverall);
                rate.RateDate = DateTime.Now;
                db.Entry(rate.UserWhoGetRate).State = EntityState.Modified;
                db.Notifications.Add(new NotificationModel {Project = rate.Project, NotificationType = NotificationTypes.NowaOcena, IsSeen = false, DateSend = DateTime.Now, NotificationReceiver = rate.UserWhoGetRate, Content = string.Format("Użytkownik {0} wystawił Ci ocenę za projekt {1}. Średnia ocen to: {2}", rate.UserWhoAddRate.Username, rate.Project.ProjectTitle, rate.AverageRate)});
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

        public ActionResult UserRates(int userId)
        {
            using(DbContextModel db = new DbContextModel())
            {
                var ratesConnectedWithUser = db.Rates
                                                .Include("UserWhoGetRate")
                                                .Include("UserWhoAddRate")
                                                .Include("Project")
                                                .OrderByDescending(x=> x.RateDate)
                                                .Where(x => x.UserWhoGetRate.UserId.Equals(userId)).ToList();
                return PartialView("_userRates", ratesConnectedWithUser);

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