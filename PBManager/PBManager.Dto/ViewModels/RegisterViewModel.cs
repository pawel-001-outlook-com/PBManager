﻿using System.ComponentModel.DataAnnotations;

namespace PBManager.Dto.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        [Display(Name = "User firstname")] public string FirstName { get; set; }

        [Display(Name = "User surname")] public string Surname { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmation password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmationPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}