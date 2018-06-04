using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    .Include("Tasks.CurrentWorkerAtTask")
                    .Include("Tasks.UserWhoAddTask")
                    .Include("UsersBelongsToProjectCollection")
                    .Where(x => x.ProjectId.Equals(projectId))
                    .FirstOrDefault();


                var categoriesList = db.Categories.ToList();
                var skills = db.Skills.ToList();

                var projectCategoryId = Convert.ToInt32(project.ProjectCategory);
                project.CategoriesModel = categoriesList.Where(x => x.CategoryId.Equals(projectCategoryId)).FirstOrDefault();
                if (project.SkillsRequiredToProject != null)
                {
                    project.SkillsRequiredToProjectArray = project.SkillsRequiredToProject.Split(',').ToArray();
                    project.SkillsRequiredToProjectCollection = new Collection<SkillsModel>();
                    foreach (var skillId in project.SkillsRequiredToProjectArray)
                    {
                        var skillIdInt = Convert.ToInt32(skillId);
                        var skill = skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                        project.SkillsRequiredToProjectCollection.Add(skill);
                    }
                }

                return View(project);
            }
        }

        public ActionResult KanbanBoard(int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var project = db.Projects
                    .Include("ProjectOwner")
                    .Include("SkillsRequiredToProjectCollection")
                    .Include("CategoriesModel")
                    .Include("Tasks.CurrentWorkerAtTask")
                    .Include("Tasks.UserWhoAddTask")
                    .Include("UsersBelongsToProjectCollection")
                    .Where(x => x.ProjectId.Equals(projectId))
                    .FirstOrDefault();


                return View(project);
            }
        }
    }
}