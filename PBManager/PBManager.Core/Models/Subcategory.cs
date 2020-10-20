using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Core.Models
{
    public class Subcategory : BaseEntity
    {
        public string Name { get; set; }

        // [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();
        //
        // public bool CanEdit { get; set; } = true;
        //
        // public bool Enabled { get; set; } = true;
    }
}
