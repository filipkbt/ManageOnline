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

        public ActionResult UpdateTaskPosition(string itemIds)
        {
            int count = 1;

            List<int> itemIdList = new List<int>();
            itemIdList = itemIds.Split(",".ToCharArray(),StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            using (DbContextModel db = new DbContextModel())
            {
                foreach (var itemId in itemIdList)
                { 
                    TaskModel task = db.Tasks.Where(x => x.TaskId.Equals(itemId)).FirstOrDefault();
                    task.RowNumber = count;
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    count++;
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}