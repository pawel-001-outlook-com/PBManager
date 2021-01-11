using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PBManager.Core.Models
{
    public class Account : BaseEntity
    {
        [Display(Name = "Account name")]
        public string Name { get; set; }

        [Display(Name = "Initial balance")]
        public double InitialBalance { get; set; } = 0;

        public double Balance { get; set; } = 0;

        public virtual ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();

        public int UserId { get; set; }
        public User User { get; set; }

        public string Description { get; set; }
    }
}
