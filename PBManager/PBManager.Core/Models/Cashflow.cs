using System;
using System.ComponentModel.DataAnnotations;

namespace PBManager.Core.Models
{
    public class Cashflow : BaseEntity
    {
        [Display(Name = "Cashflow name")] public string Name { get; set; }

        public string Description { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "value must be positive")]
        public double Value { get; set; }

        [Display(Name = "Accounting date")] public DateTime AccountingDate { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }


        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int? SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }

        public int? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}