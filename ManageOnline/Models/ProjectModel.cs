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
        [Description("Oczekuje na oferty")]
        WaitingForOffers,
        [Description("W toku")]
        InProgress,
        [Description("Zakończone")]
        Finished
    }

    public enum ProjectManagementMethodology
    {
        Kanban = 1,
        Scrum = 2
    }

    public class ProjectModel
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        [DisplayName("Nazwa projektu")]
        public string ProjectTitle { get; set; }
        [Required]
        [DisplayName("Opis")]
        public string ProjectDescription { get; set; }
        [Required]
        [DisplayName("Zakres prac do wykonania")]
        public string ProjectResponsibilities { get; set; }

        public DateTime ProjectCreationDate { get; set; }

        public DateTime? ProjectStartDate { get; set; }

        public DateTime? ProjectFinishDate { get; set; }

        public UserBasicModel ProjectOwner { get; set; }

        public string UsersBelongsToProject { get; set; }
        public virtual ICollection<UserBasicModel> UsersBelongsToProjectCollection { get; set; }
        [NotMapped]
        public string[] UsersBelongsToProjectArray { get; set; }

        public ICollection<TaskModel> Tasks { get; set; }

        public string SkillsRequiredToProject { get; set; }
        public virtual ICollection<SkillsModel> SkillsRequiredToProjectCollection { get; set; }
        [NotMapped]
        public string[] SkillsRequiredToProjectArray { get; set; }

        public virtual ICollection<OfferToProjectModel> OffersToProject { get; set; }

        public virtual ICollection<RateModel> RatesCollection { get; set; }

        public ProjectStatus ProjectStatus { get; set; }

        public ProjectManagementMethodology ProjectManagementMethodology { get; set; }

        public virtual ICollection<ScrumSprintModel> ScrumSprints { get; set; }

        public string ProjectBudget { get; set; }

        [NotMapped]
        public virtual ICollection<SkillsModel> CategoriesToProjectCollection { get; set; }
        [NotMapped]
        public string[] CategoriesToProjectArray { get; set; }
        public virtual CategoriesModel ProjectCategory { get; set; }

        public bool IsRequiredManager { get; set; }

        public UserBasicModel Manager { get; set; }

    }
}