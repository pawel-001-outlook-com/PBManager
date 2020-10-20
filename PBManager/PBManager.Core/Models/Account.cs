using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Core.Models
{
    public class Account : BaseEntity
    {
        // [Required]
        // [StringLength(30)]
        [Display(Name = "Account name")]
        public string Name { get; set; }


        // [Required]
        [Display(Name = "Initial balance")]
        public double InitialBalance { get; set; } = 0;


        public double Balance { get; set; } = 0;


        public virtual ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();


        // [ForeignKey("User")]
        public int UserId { get; set; } 
        public User User { get; set; }

        // [Required]
        // [StringLength(50)]
        public string Description { get; set; }
    }
}
