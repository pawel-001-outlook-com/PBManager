using PBManager.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PBManager.Dto.ViewModels
{
    public class ProjectViewModel
    {
        [Display(Name = "Project Id")]
        [Required]
        public int Id { get; set; }

        [Display(Name = "Project name")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Project start date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Display(Name = "Project description")]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public ICollection<Cashflow> Cashflows { get; set; }

        [Display(Name = "Project total value")]
        public double ProjectTotalValue
        {
            get => Cashflows != null ? Cashflows.Sum(c => c.Value) : 0;
        }

        [Required]
        [RegularExpression(@"[0-9]+([\,]*[0-9]+)*", ErrorMessage = "Only integer or double values are accepted")]
        public string Budget { get; set; }



    }
}
