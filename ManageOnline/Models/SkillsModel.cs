using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{

    public enum Skills
    {
        Programowanie = 1,
        Pisanie = 2
    }

    public class SkillsModel
    {   [Key]
        public int SkillId { get; set; }

        public Skills SkillName { get; set; }
    }
}