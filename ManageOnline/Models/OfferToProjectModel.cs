using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class OfferToProjectModel
    {
        [Key]
        public int OfferToProjectId { get; set; }

        public int ProjectId { get; set; }
        public virtual ProjectModel ProjectWhereOfferWasAdded { get; set; }

        public virtual UserBasicModel UserWhoAddOffer { get; set; }

        public DateTime AddOfferDate { get; set; }
        [DisplayName("Opis")]
        public string Description { get; set; }
        [DisplayName("Koszt")]
        public double Budget { get; set; }
        [DisplayName("Czas realizacji")]
        public int EstimatedTimeToFinishProject { get; set; }

        public UserBasicModel WorkerProposedToProject { get; set; }
        public virtual ICollection<UserBasicModel> WorkersProposedToProjectCollection { get; set; }

        [NotMapped]
        public string[] WorkersProposedToProjectArray { get; set; }
    }
}