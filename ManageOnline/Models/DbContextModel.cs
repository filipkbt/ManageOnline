using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
        public class DbContextModel : DbContext
        {
            public DbSet<UserBasicModel> UserAccounts { get; set; }

            public DbSet<ProjectModel> Projects { get; set; }

            public DbSet<SkillsModel>  Skills { get; set; }

            public DbSet<CommentModel> Comments { get; set; }

            public DbSet<TaskModel> Tasks { get; set; }

            public DbSet<OfferToProjectModel> OfferToProjectModels { get; set; } 
    }
}