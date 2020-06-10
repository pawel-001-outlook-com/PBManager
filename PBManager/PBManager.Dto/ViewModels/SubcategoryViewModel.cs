using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;

namespace PBManager.Dto.ViewModels
{
    class SubcategoryViewModel
    {
        public Subcategory Subcategory { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
