using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
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

        public ICollection<UserBasicModel> UsersBelongsToProject { get; set; }

        public ICollection<TaskModel> Tasks { get; set; }

        public ICollection<SkillsModel> SkilsRequiredToProject { get; set; }

        public ICollection<OfferToProjectModel> OffersToProject { get; set; }

        [DefaultValue(false)]
        public bool IsProjectAllowedToSomeone { get; set; }

        [DefaultValue(false)]
        public bool IsProjectInProgress { get; set; }

        [DefaultValue(false)]
        public bool IsProjectFinished { get; set; }

    }
}