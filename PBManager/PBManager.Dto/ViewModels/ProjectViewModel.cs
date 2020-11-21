using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PBManager.Core.Models;

namespace PBManager.Dto.ViewModels
{
    public class ProjectViewModel
    {
        [Display(Name = "Project Id")]
        [Required]
        public int Id { get; set; }

        [Display(Name = "Project name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Project start date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "Project description")]
        [StringLength(100)]
        public string Description { get; set; }

        [Required] public int UserId { get; set; }

        public ICollection<Cashflow> Cashflows { get; set; }

        [Display(Name = "Project total value")]
        public double ProjectTotalValue => Cashflows != null ? Cashflows.Sum(c => c.Value) : 0;


        public double? Budget { get; set; }


        [Display(Name = "Free budget")]
        public double? ProjectFreeBudget => Budget != null ? Budget - ProjectTotalValue : 0;

        [Display(Name = "Budget consumed")]
        public double ProjectBudgetConsumed => Budget != null ? ProjectTotalValue / Budget.Value * 100 : 0;
    }
}