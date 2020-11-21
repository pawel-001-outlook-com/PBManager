using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using PBManager.Core.Models;

namespace PBManager.Dto.ViewModels
{
    public class AccountViewModel
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "Account name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Initial balance")]
        public double InitialBalance { get; set; }


        public double Balance => Cashflows != null ? Cashflows.Sum(c => c.Value) : 0;


        [Display(Name = "Account description")]
        [StringLength(50)]
        public string Description { get; set; }

        [Required] [Display(Name = "User Id")] public int UserId { get; set; }

        [Required]
        [Display(Name = "Account Id")]
        public int Id { get; set; }

        [JsonIgnore] public ICollection<Cashflow> Cashflows { get; set; }
    }
}