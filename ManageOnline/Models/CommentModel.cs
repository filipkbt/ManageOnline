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

        public int UserId { get; set; }
        public UserBasicModel CommentUser { get; set; }

        public string CommentDescription { get; set; }

        public int TaskId { get; set; }
        public virtual TaskModel TaskWhereCommentBelong { get; set; }

        public int ProjectId { get; set; }
        public virtual ProjectModel ProjectWhereCommentBelong { get; set; }

        public DateTime DateWhenCommentWasAdded { get; set; }
    }
}