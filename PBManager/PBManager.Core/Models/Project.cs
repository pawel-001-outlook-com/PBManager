using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Core.Models
{
    public class Project : BaseEntity
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "Project name")]
        public string Name { get; set; }

        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Finish date")]
        public DateTime? FinishDate { get; set; }

        public double? Budget { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public bool Enabled { get; set; }

        public virtual ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();

    }
}
