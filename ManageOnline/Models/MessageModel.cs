using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class MessageModel
    {
        [Key]
        public int MessageId { get; set; }

        [DefaultValue(false)]
        public bool IsSeen { get; set; }

        public DateTime DateSend { get; set; }

        public virtual UserBasicModel Sender { get; set; }

        public virtual UserBasicModel Receiver { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}