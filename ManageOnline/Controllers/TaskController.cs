using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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

        public ActionResult UpdateTaskPosition(string column1,string column2)
        {
            var column1Tasks = column1.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var column2Tasks = column2.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            using (DbContextModel db = new DbContextModel())
            {
                int counter1 = 1;
                foreach (var itemId in column1Tasks)
                {
                    TaskModel task = db.Tasks.Where(x => x.TaskId.Equals(itemId)).FirstOrDefault();
                    task.RowNumber = counter1;
                    task.ColumnNumber = 1;
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    counter1++;
                }
                int counter2 = 1;
                foreach (var itemId in column2Tasks)
                {
  
                    TaskModel task = db.Tasks.Where(x => x.TaskId.Equals(itemId)).FirstOrDefault();
                    task.RowNumber = counter2;
                    task.ColumnNumber = 2;
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    counter2++;
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}