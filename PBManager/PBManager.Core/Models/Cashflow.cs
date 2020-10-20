using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Core.Models
{
    public class Cashflow : BaseEntity
    {
        // [StringLength(1)]
        // [Required]
        // public string Type { get; set; }

        // [Required]
        // [StringLength(50)]

        public string Name { get; set; }

        public string Description { get; set; }

        // [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "value must be positive")]
        public double Value { get; set; }

        // [Display(Name = "Inclusion date")]
        // public DateTime InclusionDate { get; set; }

        // [Display(Name = "Accounting date")]
        public DateTime AccountingDate { get; set; }

        // [ForeignKey("Account")]
        public int AccountId { get; set; }

        public Account Account { get; set; }

        // [ForeignKey("Category")]
        public int? CategoryId { get; set; }

        public Category Category { get; set; }
        
        // [ForeignKey("Subcategory")]
        public int? SubcategoryId { get; set; }

        public Subcategory Subcategory { get; set; }

        // [ForeignKey("Project")]
        public int? ProjectId { get; set; }

        public Project Project { get; set; }

        //
        // [ForeignKey("Category")]
        // public int? CategoryId { get; set; }
        //
        // public Category Category { get; set; }

    }
}
