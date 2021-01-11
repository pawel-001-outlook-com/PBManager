using PBManager.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"[0-9]+([\,]*[0-9]+)*", ErrorMessage = "Only integer or double values are accepted")]
        public string Value { get; set; }

        [Required]
        [Display(Name = "Account name")]
        public int AccountId { get; set; }

        public Account Account { get; set; }

        [Display(Name = "Category Name")]
        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Category name")]
        public string CategoryName
        {
            get => Category != null ? Category.Name : "";
        }

        [Display(Name = "Subcategory Name")]
        public int? SubcategoryId { get; set; }

        public Subcategory Subcategory { get; set; }

        [Display(Name = "Subcategory name")]
        public string SubcategoryName
        {
            get => Subcategory != null ? Subcategory.Name : "";
        }

        [Display(Name = "Project name")]
        public int? ProjectId { get; set; }

        public Project Project { get; set; }

        [Display(Name = "Project name")]
        public string ProjectName
        {
            get => Project != null ? Project.Name : "";
        }

        public Cashflow Cashflow { get; set; } = new Cashflow();
        public IEnumerable<Account> Accounts { get; set; } = new List<Account>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public IEnumerable<Project> Projects { get; set; } = new List<Project>();
    }
}
