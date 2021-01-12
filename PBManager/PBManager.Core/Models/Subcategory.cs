using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PBManager.Core.Models
{
    public class Subcategory : BaseEntity
    {
        [Display(Name = "Subcategory name")]
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();
    }
}
