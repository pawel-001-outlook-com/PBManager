using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PBManager.Core.Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<Project> Projects { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
