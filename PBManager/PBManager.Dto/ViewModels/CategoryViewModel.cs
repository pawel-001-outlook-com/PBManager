using PBManager.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PBManager.Dto.ViewModels
{
    public class CategoryViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();

        public ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();

    }
}
