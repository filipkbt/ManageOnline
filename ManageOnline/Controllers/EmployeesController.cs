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
    }
}