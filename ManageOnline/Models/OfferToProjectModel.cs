using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class OfferToProjectModel
    {
        [Key]
        public int OfferToProjectId { get; set; }

        public ProjectModel ProjectWhereOfferWasAdded { get; set; }

        public UserBasicModel UserWhoAddOffer { get; set; }

        public DateTime AddOfferDate { get; set; }

        public string Description { get; set; }

        public double Budget { get; set; }

        public int EstimatedTimeToFinishProject { get; set; }

        public ICollection<UserBasicModel> WorkersProposedToProject { get; set; }
    }
}