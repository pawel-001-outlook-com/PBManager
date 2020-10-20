using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Core.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public virtual ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();

        // public bool CanEdit { get; set; } = true;
        //
        // public bool Enabled { get; set; } = true;

        // [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        public string Description { get; set; }
    }
}
