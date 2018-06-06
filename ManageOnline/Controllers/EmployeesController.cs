using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageOnline.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult EmployeesList()
        {
            DbContextModel db = new DbContextModel();

            var workersList = db.UserAccounts.Where(u => u.Role == Roles.Pracownik);

            return View(workersList);
        }

        public ActionResult EmployeesFromProject(int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                project.UsersBelongsToProjectArray = project.UsersBelongsToProject.Split(',').ToArray();

                foreach (var userId in project.UsersBelongsToProjectArray)
                {
                    int userIdInt = Convert.ToInt32(userId);
                    var user = db.UserAccounts.Where(x => x.UserId.Equals(userIdInt)).FirstOrDefault();
                    project.UsersBelongsToProjectCollection.Add(user);
                }
                return View(project);
            }
        }
    }
}