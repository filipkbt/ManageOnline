using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public enum Roles : int
    {
        Pracownik,
        Manager,
        Klient
    }


    public class UserBasicModel
    {
        [Key]
        public int UserId { get; set; }
        [DisplayName("Adres email")]
        [Required(ErrorMessage = "Email jest wymagany.")]
        public string Email { get; set; }
        [DisplayName("Login")]
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        public string Username { get; set; }
        [DisplayName("Hasło")]
        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Potwierdź swoje hasło.")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [DisplayName("Rola")]
        [Required(ErrorMessage = "Podanie roli konta jest wymagane.")]
        public Roles Role { get; set; }
        [DisplayName("Profesja")]
        public string DisplayedRole { get; set; }
        [DisplayName("Numer kontaktowy")]
        public string MobileNumber { get; set; }
        [DisplayName("Opis")]
        public string Description { get; set; }
        [DisplayName("Zdjęcie profilowe")]
        public byte[] UserPhoto { get; set; }

        [DisplayName("Umiejętności")]
        public string Skills { get; set; }
        public virtual ICollection<SkillsModel> SkillsCollection { get; set; }
        [NotMapped]
        public string[] SkillsArray { get; set; }
        
        public double AverageRate { get; set; }

        public int FinishedProjects{ get; set; }

        public int ProjectsInProgress { get; set; }

        [NotMapped]
        public bool IsRatedAtCurrentProject { get; set; }
    }
}