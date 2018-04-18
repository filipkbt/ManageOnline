﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }

        public ProjectModel ProjectId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public DateTime TaskCreationDate { get; set; }
        
        public int WorkerId { get; set; }

        public int CurrentWorketAtTaskId { get; set; }

        public virtual UserBasicModel CurrentWorkerAtTask { get; set; }
    }
}