using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Core.Models
{
    public class SubcategoryBudget : BaseEntity
    {
        [ForeignKey("Subcategory")]
        public int SubcategoryID { get; set; }
        public virtual Subcategory Subcategory { get; set; }
    }
}
