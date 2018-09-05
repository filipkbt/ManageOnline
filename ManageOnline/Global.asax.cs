using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ManageOnline
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using(DbContextModel db = new DbContextModel())
            {
                var adminAccount = db.UserAccounts.Where(x => x.Role == Roles.Admin).FirstOrDefault();
                if(adminAccount == null)
                {
                    var password = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
                                          .ComputeHash(Encoding.UTF8.GetBytes("filip123")));
                    var confirmPassword = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
                                                 .ComputeHash(Encoding.UTF8.GetBytes("filip123")));
                    adminAccount = new UserBasicModel() { Username = "administrator", Email = "filip.admin@wp.pl", MobileNumber = "511 114 421", Role = Roles.Admin };
                    adminAccount.Password = password;
                    adminAccount.ConfirmPassword = confirmPassword;
                    byte[] image = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Images/icons8-administrator-mężczyzna-480.png"));
                    adminAccount.UserPhoto = image;

                    db.UserAccounts.Add(adminAccount);
                    db.SaveChanges();
                }
            }
        }
    }
}
