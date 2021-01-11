﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PBManager.Core.Models
{
    public class Project : BaseEntity
    {
        [Display(Name = "Project name")]
        public string Name { get; set; }

        [Display(Name = "Project start date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Finish date")]
        public DateTime? FinishDate { get; set; }

        [Display(Name = "Budget value")]
        public double Budget { get; set; }

        [Display(Name = "Project description")]
        [StringLength(100)]
        public string Description { get; set; }

        public ICollection<Cashflow> Cashflows { get; set; } = new List<Cashflow>();

        public int UserId { get; set; }
        public User User { get; set; }

        [NotMapped]
        public double BudgetConsumed
        {
            get => Budget != null ? ProjectTotalValue / Budget * 100 : 0;
        }

        [NotMapped]
        public double ProjectTotalValue
        {
            get => Cashflows != null ? Cashflows.Sum(c => c.Value) : 0;
        }

    }
}
