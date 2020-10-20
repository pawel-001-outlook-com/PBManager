using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;

namespace PBManager.Dto.ViewModels
{
    public class CategoryViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();

        public ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();

    }
}
