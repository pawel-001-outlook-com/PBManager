using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;

namespace PBManager.Dto.ViewModels
{
    public class CategoryChartReportViewModel
    {
        [Required]
        [Display(Name = "User Id")]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Account Id")]
        public int AccountId { get; set; }

        [Required]
        [Display(Name = "Category Id")]
        public int? CategoryId { get; set; }

        [Display(Name = "Subcategory Id")]
        public int? SubcategoryId { get; set; }

        [Display(Name = "Project Id")]
        public int? ProjectId { get; set; }

        [Required]
        [Display(Name = "Date from")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Date to")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Report form (per day, per month")]
        public int reportForm { get; set; }



        public IEnumerable<Account> Accounts { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Cashflow> Cashflows { get; set; } = new List<Cashflow>();



        public bool IfData { get; set; } = false;
    }
}
