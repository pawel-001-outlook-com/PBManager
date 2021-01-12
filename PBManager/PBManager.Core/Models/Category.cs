using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PBManager.Core.Models
{
    public class Category : BaseEntity
    {
        [Display(Name = "Category name")]
        public string Name { get; set; }

        public string Type { get; set; }

        public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();

        public virtual ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();

        public int UserID { get; set; }
        public User User { get; set; }

        [Display(Name = "Category description")]
        public string Description { get; set; }
    }
}
