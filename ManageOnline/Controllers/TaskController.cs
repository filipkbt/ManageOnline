using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageOnline.Models;

namespace ManageOnline.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        [HttpPost]
        public ActionResult AddTask(TaskModel task)
        {
            var gowno = task;
            
            return View(("~/Views/Dashboard/DashboardIndex.cshtml"));
        }
    }
}