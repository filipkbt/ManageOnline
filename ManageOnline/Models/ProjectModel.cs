using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public enum ProjectStatus
    {
        [Description("Waiting for offers")]
        WaitingForOffers,
        [Description("In Progress")]
        InProgress,
        [Description("Finished")]
        Finished
    }

    public class ProjectModel
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string ProjectTitle { get; set; }
        [Required]
        public string ProjectDescription { get; set; }

        public DateTime ProjectCreationDate { get; set; }

        public DateTime? ProjectStartDate { get; set; }

        public DateTime? ProjectFinishDate { get; set; }

        public UserBasicModel ProjectOwner { get; set; }

        public string UsersBelongsToProject { get; set; }
        public virtual ICollection<UserBasicModel> UsersBelongsToProjectCollection { get; set; }

        public ICollection<TaskModel> Tasks { get; set; }


        public string SkillsRequiredToProject { get; set; }
        public virtual ICollection<SkillsModel> SkillsRequiredToProjectCollection { get; set; }

        public ICollection<OfferToProjectModel> OffersToProject { get; set; }

        public ProjectStatus ProjectStatus { get; set; }

        public string ProjectBudget { get; set; }

        public string ProjectCategory { get; set; }
        public virtual CategoriesModel CategoriesModel { get; set; }

    }
}