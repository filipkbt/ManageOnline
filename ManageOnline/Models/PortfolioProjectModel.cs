﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class PortfolioProjectModel
    {   [Key]
        public int PortfolioProjectId { get; set; }

        public virtual UserBasicModel EmployeeId { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectDescription { get; set; }
        public string ProjectLink { get; set; }
        public byte[] ProjectImage { get; set; }
    }
}