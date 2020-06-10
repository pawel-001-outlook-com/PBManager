using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Core.Models
{
    public class Account:BaseEntity
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "Account name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Account kind")]
        public AccountKind AccountKind { get; set; }

        [Required]
        [Display(Name = "Initial balance")]
        public double InitialBalance { get; set; }

        public double Balance { get; set; }
        public virtual ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();
        public bool Enabled { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

    }
}
