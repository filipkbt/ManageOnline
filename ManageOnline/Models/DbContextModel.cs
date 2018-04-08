using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
        public class DbContextModel : DbContext
        {
            public DbSet<UserBasicModel> userAccount { get; set; }
        }
}