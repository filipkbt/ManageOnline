using ManageOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageOnline.ViewModels
{
    public class ProjectUserViewModel
    {
        public ProjectModel Project { get; set; }

        public UserBasicModel ProjectOwner { get; set; }
    }
}