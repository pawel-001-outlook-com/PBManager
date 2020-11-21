using System.ComponentModel.DataAnnotations;

namespace PBManager.Dto.ViewModels
{
    public class LoginViewModel
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }

        [Display(Name = "Remember Me")]
        [Required]
        public bool RememberMe { get; set; }
    }
}