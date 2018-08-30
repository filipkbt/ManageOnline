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
            using (DbContextModel db = new DbContextModel())
            {
                var workersList = db.UserAccounts.Where(u => u.Role == Roles.Pracownik).ToList();

                var skills = db.Skills.ToList();
                MultiSelectList skillsList = new MultiSelectList(skills, "SkillId", "SkillName");

                if ((TempData["Skills"] as MultiSelectList) != null)
                {
                    var selectedSkillsSelectList = TempData["Skills"] as MultiSelectList;
                    var selectedSkills = selectedSkillsSelectList.Items;
                }

                TempData["Skills"] = skillsList;

                foreach (var employee in workersList)
                {
                    if (employee.Skills != null)
                    {
                        employee.SkillsArray = employee.Skills.Split(',').ToArray();
                        foreach (var skillId in employee.SkillsArray)
                        {
                            var skillIdInt = Convert.ToInt32(skillId);
                            var skill = db.Skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                            employee.SkillsCollection.Add(skill);
                        }
                    }
                }

                return View(workersList);
            }

        }

        [HttpPost]
        public ActionResult EmployeesList(FormCollection form)
        {
            using (DbContextModel db = new DbContextModel())
            {

                var workersList = db.UserAccounts.Where(u => u.Role == Roles.Pracownik).ToList();

                var skills = db.Skills.ToList();
                MultiSelectList skillsList = new MultiSelectList(skills, "SkillId", "SkillName");

                TempData["Skills"] = skillsList;

                foreach (var employee in workersList)
                {
                    if (employee.Skills != null)
                    {
                        employee.SkillsArray = employee.Skills.Split(',').ToArray();
                        foreach (var skillId in employee.SkillsArray)
                        {
                            var skillIdInt = Convert.ToInt32(skillId);
                            var skill = db.Skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                            employee.SkillsCollection.Add(skill);
                        }
                    }
                }
                List<UserBasicModel> filteredDataContext = new List<UserBasicModel>();
                string rateValue= form["rateValueText"];
                string rateValuePreparedToDoubleConvert = rateValue.Replace(".", ",");
                double minimumAverageRate = 0;
                if (rateValue != "")
                {
                    minimumAverageRate = Convert.ToDouble(rateValuePreparedToDoubleConvert);
                }

                if (form["Skills"] != null || form["rateValueText"] != null)
                {
                    if (form["Skills"] != null)
                    {
                        var selectedSkills = form["Skills"];
                        var selectedSkillsArray = selectedSkills.Split(',').ToArray();
                        int countOfSelectedSkills = selectedSkillsArray.Count();

                        foreach (var employee in workersList)
                        {
                            bool flag = false;
                            for (int i = 0; i < countOfSelectedSkills; i++)
                            {
                                if (employee.SkillsArray.Contains(selectedSkillsArray[i]))
                                    flag = true;
                                else
                                    flag = false;
                            }
                            if (flag)
                                filteredDataContext.Add(employee);
                        }
                        if (form["rateValueText"] != null)
                        {
                            var filteredDataContextWithCheckedAverageRateAndSkills = filteredDataContext.Where(x => x.AverageRate >= minimumAverageRate).ToList();
                            return View(filteredDataContextWithCheckedAverageRateAndSkills);
                        }
                        else
                        {
                            return View(filteredDataContext);
                        }
                    }
                    else if (form["rateValueText"] != null)
                    {
                        var filteredDataContextWithCheckedAverageRate = workersList.Where(x => x.AverageRate >= minimumAverageRate).ToList();
                        return View(filteredDataContextWithCheckedAverageRate);
                    }
                }
                return View(workersList);
            }
        }

        public ActionResult EmployeesFromProject(int projectId)
        {
            using (DbContextModel db = new DbContextModel())
            {
                var project = db.Projects.Where(x => x.ProjectId.Equals(projectId)).FirstOrDefault();
                project.UsersBelongsToProjectArray = project.UsersBelongsToProject.Split(',').ToArray();
                ViewBag.ProjectManagementMethodology = project.ProjectManagementMethodology;
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