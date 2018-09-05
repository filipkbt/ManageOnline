using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageOnline.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult AdminDashboard()
        {
            using (DbContextModel db = new DbContextModel())
            {
                int UserId = Convert.ToInt32(Session["UserId"]);
                UserBasicModel adminAccount = db.UserAccounts.FirstOrDefault(u => u.UserId.Equals(UserId));

                return View(adminAccount);
            }
        }

        public ActionResult ShowClientsAndEmployees()
        {
            using (DbContextModel db = new DbContextModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var clientsAndEmployees = db.UserAccounts.Where(x => x.Role == Roles.Pracownik || x.Role == Roles.Klient).ToList();

                var skills = db.Skills.ToList();
                MultiSelectList skillsList = new MultiSelectList(skills, "SkillId", "SkillName");

                if ((TempData["Skills"] as MultiSelectList) != null)
                {
                    var selectedSkillsSelectList = TempData["Skills"] as MultiSelectList;
                    var selectedSkills = selectedSkillsSelectList.Items;
                }


                TempData["Skills"] = skillsList;

                foreach (var user in clientsAndEmployees)
                {
                    user.SkillsCollection = new List<SkillsModel>();
                    if (user.Skills != null)
                    {
                        user.SkillsArray = user.Skills.Split(',').ToArray();
                        foreach (var skillId in user.SkillsArray)
                        {
                            var skillIdInt = Convert.ToInt32(skillId);
                            var skill = db.Skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                            user.SkillsCollection.Add(skill);
                        }
                    }
                }

                return View(clientsAndEmployees);
            }
        }

        [HttpPost]
        public ActionResult ShowClientsAndEmployees(FormCollection form)
        {
            using (DbContextModel db = new DbContextModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var clientsAndEmployees = db.UserAccounts.Where(x => x.Role == Roles.Manager).ToList();

                var skills = db.Skills.ToList();
                MultiSelectList skillsList = new MultiSelectList(skills, "SkillId", "SkillName");

                TempData["Skills"] = skillsList;

                foreach (var user in clientsAndEmployees)
                {
                    if (user.Skills != null)
                    {
                        user.SkillsArray = user.Skills.Split(',').ToArray();
                        foreach (var skillId in user.SkillsArray)
                        {
                            var skillIdInt = Convert.ToInt32(skillId);
                            var skill = db.Skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                            user.SkillsCollection.Add(skill);
                        }
                    }
                }
                List<UserBasicModel> filteredDataContext = new List<UserBasicModel>();
                string rateValue = form["rateValueText"];
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

                        foreach (var user in clientsAndEmployees)
                        {
                            bool flag = false;
                            for (int i = 0; i < countOfSelectedSkills; i++)
                            {
                                if (user.SkillsArray.Contains(selectedSkillsArray[i]))
                                    flag = true;
                                else
                                    flag = false;
                            }
                            if (flag)
                                filteredDataContext.Add(user);
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
                        var filteredDataContextWithCheckedAverageRate = clientsAndEmployees.Where(x => x.AverageRate >= minimumAverageRate).ToList();
                        return View(filteredDataContextWithCheckedAverageRate);
                    }
                }

                return View(clientsAndEmployees);
            }
        }

        public ActionResult ShowManagers()
        {
            using (DbContextModel db = new DbContextModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var managers = db.UserAccounts.Where(x => x.Role == Roles.Manager).ToList();
                var skills = db.Skills.ToList();
                MultiSelectList skillsList = new MultiSelectList(skills, "SkillId", "SkillName");

                if ((TempData["Skills"] as MultiSelectList) != null)
                {
                    var selectedSkillsSelectList = TempData["Skills"] as MultiSelectList;
                    var selectedSkills = selectedSkillsSelectList.Items;
                }

                TempData["Skills"] = skillsList;

                foreach (var manager in managers)
                {
                    if (manager.Skills != null)
                    {
                        manager.SkillsArray = manager.Skills.Split(',').ToArray();
                        foreach (var skillId in manager.SkillsArray)
                        {
                            var skillIdInt = Convert.ToInt32(skillId);
                            var skill = db.Skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                            manager.SkillsCollection.Add(skill);
                        }
                    }
                }

                return View(managers);
            }
        }

        [HttpPost]
        public ActionResult ShowManagers(FormCollection form)
        {
            using (DbContextModel db = new DbContextModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                var managers = db.UserAccounts.Where(x => x.Role == Roles.Manager).ToList();

                var skills = db.Skills.ToList();
                MultiSelectList skillsList = new MultiSelectList(skills, "SkillId", "SkillName");

                TempData["Skills"] = skillsList;

                foreach (var manager in managers)
                {
                    if (manager.Skills != null)
                    {
                        manager.SkillsArray = manager.Skills.Split(',').ToArray();
                        foreach (var skillId in manager.SkillsArray)
                        {
                            var skillIdInt = Convert.ToInt32(skillId);
                            var skill = db.Skills.Where(x => x.SkillId.Equals(skillIdInt)).FirstOrDefault();
                            manager.SkillsCollection.Add(skill);
                        }
                    }
                }
                List<UserBasicModel> filteredDataContext = new List<UserBasicModel>();
                string rateValue = form["rateValueText"];
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

                        foreach (var manager in managers)
                        {
                            bool flag = false;
                            for (int i = 0; i < countOfSelectedSkills; i++)
                            {
                                if (manager.SkillsArray.Contains(selectedSkillsArray[i]))
                                    flag = true;
                                else
                                    flag = false;
                            }
                            if (flag)
                                filteredDataContext.Add(manager);
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
                        var filteredDataContextWithCheckedAverageRate = managers.Where(x => x.AverageRate >= minimumAverageRate).ToList();
                        return View(filteredDataContextWithCheckedAverageRate);
                    }
                }

                return View(managers);
            }
        }

        public ActionResult AddManagerAccount()
        {
            using (DbContextModel db = new DbContextModel())
            {
                return View();
            }
        }
    }
}