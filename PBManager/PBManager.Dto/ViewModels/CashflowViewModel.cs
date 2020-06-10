using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;

namespace PBManager.Dto.ViewModels
{
    class CashflowViewModel
    {
        public Cashflow Cashflow { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}
