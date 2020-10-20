using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Dto.ViewModels
{
    public class ProjectDto
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        public string sStartDate { get; set; }


        public string Description { get; set; }

        public int UserId { get; set; }

        public double? TotalExpanses{ get; set; }

        public double ProjectTotalValue{ get; set; }

        public double? Budget { get; set; }

        public double? ProjectFreeBudget{ get; set; }

        public double ProjectBudgetConsumed{ get; set; }
    }
}
