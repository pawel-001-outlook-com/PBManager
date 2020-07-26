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
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "")]
        public double Value { get; set; }

        public DateTime AccountingDate { get; set; }

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
