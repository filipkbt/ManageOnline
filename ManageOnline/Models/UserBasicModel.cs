using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManageOnline.Models
{
    public enum Roles
    {
        Pracownik,
        Manager,
        Klient
    }
    public class UserBasicModel
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Email jest wymagany.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Potwierdź swoje hasło.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Podanie roli konta jest wymagane.")]
        public Roles Role { get; set; }
    }
}