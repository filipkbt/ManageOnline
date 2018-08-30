using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class CommentModel
    {   [Key]
        public int CommentId { get; set; }

        public UserBasicModel UserWhoAddComment { get; set; }

        public string CommentDescription { get; set; }

        public virtual TaskModel TaskWhereCommentBelong { get; set; }

        public virtual ProjectModel ProjectWhereCommentBelong { get; set; }

        public DateTime DateWhenCommentWasAdded { get; set; }

        public virtual CommentModel CommentConnectedWithSelectedComment { get; set; }

    }
}