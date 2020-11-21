using System.Collections.Generic;

namespace PBManager.Core.Models
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}