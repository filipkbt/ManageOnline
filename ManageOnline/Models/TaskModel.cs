using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{

    public enum TaskStatus : int
    {
        [Display(Name = "Nie rozpoczęte")]
        NotStarted,
        [Display(Name ="W toku")]
        InProgress,
        [Display(Name = "Zakończone")]
        Finished
    }

    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }

        [NotMapped]
        public int ProjectId { get; set; }
        public ProjectModel Project { get; set; }
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string TaskDescription { get; set; }

        public DateTime TaskCreationDate { get; set; }

        public DateTime? TaskStartDate { get; set; }

        public DateTime? TaskFinishDate { get; set; }

        public virtual UserBasicModel UserWhoAddTask { get; set; }

        public virtual UserBasicModel CurrentWorkerAtTask { get; set; }

        public virtual ICollection<CommentModel> Comments { get; set; }

        public TaskStatus TaskStatus { get; set; }

        public virtual ScrumSprintModel ScrumSprintWhereTaskBelong { get; set; }

        public int RowNumber { get; set; }

        public int? ColumnNumber { get; set; }
    }
}