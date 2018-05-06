using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManageOnline.Models;

namespace ManageOnline.Controllers
{
    public class ProjectPanelController : Controller
    {
        // GET: ProjectPanel
        public ActionResult ProjectPanel(int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var project = db.Projects
                    .Include("ProjectOwner")
                    .Include("SkillsRequiredToProjectCollection")
                    .Include("CategoriesModel")
                    .Include("Tasks")
                    .Include("UsersBelongsToProjectCollection")
                    .Where(x => x.ProjectId.Equals(projectId))
                    .FirstOrDefault();


                return View(project);
            }
            
        }
    }
}