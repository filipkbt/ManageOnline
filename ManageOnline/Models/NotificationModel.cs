using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class NotificationModel
    {
        [Key]
        public int NotificationId { get; set; }

        [DefaultValue(false)]
        public bool IsSeen { get; set; }

        public virtual UserBasicModel NotificationReceiver{ get; set; }

        public DateTime DateSend { get; set; }

        public string Content { get; set; }
    }
}