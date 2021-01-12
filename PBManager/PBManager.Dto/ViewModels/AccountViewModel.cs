using Newtonsoft.Json;
using PBManager.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"[0-9]+([\,]*[0-9]+)*", ErrorMessage = "Only integer or double values are accepted")]
        public string InitialBalance { get; set; }


        public double Balance
        {
            get => Cashflows != null ? Cashflows.Sum(c => c.Value) : 0;
        }


        [Display(Name = "Account description")]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "User Id")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Account Id")]
        public int Id { get; set; }

        [JsonIgnore]
        public ICollection<Cashflow> Cashflows { get; set; }


    }
}
