using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PBManager.Core.Models;

namespace PBManager.Dto.ViewModels
{
    public class ReportViewModel
    {
        [Required] [Display(Name = "User Id")] public int UserId { get; set; }

        [Display(Name = "Account Id")] public int AccountId { get; set; }

        [Display(Name = "Category Id")] public int? CategoryId { get; set; }

        [Display(Name = "Subcategory Id")] public int? SubcategoryId { get; set; }

        [Display(Name = "Project Id")] public int? ProjectId { get; set; }

        [Required]
        [Display(Name = "Date from")]
        public DateTime DateFrom { get; set; }

        [Required] [Display(Name = "Date to")] public DateTime DateTo { get; set; }

        public int reportForm { get; set; }


        public IEnumerable<Account> Accounts { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Cashflow> Cashflows { get; set; } = new List<Cashflow>();
    }
}