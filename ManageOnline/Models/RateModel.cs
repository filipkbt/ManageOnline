using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class RateModel
    {
        [Key]
        public int RateId { get; set; }

        public virtual ProjectModel Project { get; set; }

        public virtual UserBasicModel UserWhoAddRate { get; set; }

        public virtual UserBasicModel UserWhoGetRate { get; set; }
        [Required]
        public string Comment { get; set; }

        public int AverageRate { get; set; }

        //all
        public int Communication { get; set; }

        public int Professionalism { get; set; }

        public int MeetingTheConditions { get; set; }
        //employee
        public int? Skills { get; set; }

        public int? Punctuality { get; set; }

        public int? Quality { get; set; }
        //manager and employee by employer
        public int? ChanceToBeHiredAgain { get; set; }
        //employer by manager and employee
        public int? WantToCoworkAgain { get; set; }
        //manager
        public int? ManageSkills { get; set; }


    }
}