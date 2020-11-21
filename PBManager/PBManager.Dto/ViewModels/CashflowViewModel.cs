using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PBManager.Core.Models;

namespace PBManager.Dto.ViewModels
{
    public class CashflowViewModel
    {
        [Required]
        [Display(Name = "Cashflow Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Accounting date")]
        public DateTime AccountingDate { get; set; }

        [Required]
        [Display(Name = "Cashflow name")]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        // [Range(0.00, double.MaxValue, ErrorMessage = "value cant be smaller than 0")]
        // [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double Value { get; set; }

        [Required]
        [Display(Name = "Account Id")]
        public int AccountId { get; set; }

        public Account Account { get; set; }

        [Display(Name = "Category Id")] public int? CategoryId { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Category name")] public string CategoryName => Category != null ? Category.Name : "";

        [Display(Name = "Subcategory Id")] public int? SubcategoryId { get; set; }

        public Subcategory Subcategory { get; set; }

        [Display(Name = "Subcategory name")]
        public string SubcategoryName => Subcategory != null ? Subcategory.Name : "";

        [Display(Name = "Project Id")] public int? ProjectId { get; set; }

        public Project Project { get; set; }

        [Display(Name = "Project name")] public string ProjectName => Project != null ? Project.Name : "";

        public Cashflow Cashflow { get; set; } = new Cashflow();
        public IEnumerable<Account> Accounts { get; set; } = new List<Account>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public IEnumerable<Project> Projects { get; set; } = new List<Project>();
    }
}