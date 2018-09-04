using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class ScrumSprintModel
    {
        [Key]
        public int ScrumSprintId { get; set; }

        [Required]
        public ProjectModel Project { get; set; }

        public virtual IEnumerable<TaskModel> TasksBelongsToSprint { get; set; }

        public int ScrumSprintNumber { get; set; }

        public DateTime StartScrumSprintDate { get; set; }

        public DateTime FinishScrumSprintDate { get; set; }

        public int ScrumSprintLengthInDays { get; set; }

        public bool IsFinished { get; set; }
    }
}