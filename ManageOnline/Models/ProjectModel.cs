using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class ProjectModel
    {
        [Key]
        public int ProjectId { get; set; }

        public string ProjectTitle { get; set; }

        public string ProjectDescription { get; set; }

        public DateTime ProjectCreationDate { get; set; }

        public DateTime ProjectStartDate { get; set; }

        public DateTime ProjectFinishDate { get; set; }
        
        public UserBasicModel ProjectOwner { get; set; }

        public ICollection<UserBasicModel> UsersBelongsToProject { get; set; }

        public ICollection<TaskModel> Tasks { get; set; }

        public ICollection<SkillsModel> SkilsRequiredToProject { get; set; }

    }
}