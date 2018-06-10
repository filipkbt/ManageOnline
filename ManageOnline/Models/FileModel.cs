using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public class FileModel
    {
        [Key]
        public int FileId { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public DateTime DateUploadFile { get; set; }

        public virtual UserBasicModel UserWhoAddFile { get; set; }

        public virtual ProjectModel Project { get; set; }
    }
}