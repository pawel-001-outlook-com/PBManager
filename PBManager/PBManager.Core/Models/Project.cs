using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Core.Models
{
    public class Project : BaseEntity
    {
        // [Required]
        // [StringLength(30)]
        [Display(Name = "Project name")]
        public string Name { get; set; }

        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Finish date")]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd-MM-yyyy}")]
        public DateTime? FinishDate { get; set; }

        public double? Budget { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        // public bool Enabled { get; set; }

        public ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();

        // [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
