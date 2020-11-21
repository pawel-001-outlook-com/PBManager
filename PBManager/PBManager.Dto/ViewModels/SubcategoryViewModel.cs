using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PBManager.Core.Models;

namespace PBManager.Dto.ViewModels
{
    public class SubcategoryViewModel
    {
        [Required] public int Id { get; set; }

        [Display(Name = "Subcategory name")]
        [Required]
        public string Name { get; set; }

        [Required] public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();
        public IEnumerable<Category> Categories { get; set; }
    }
}