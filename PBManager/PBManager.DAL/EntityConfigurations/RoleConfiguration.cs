using System.Data.Entity.ModelConfiguration;
using PBManager.Core.Models;

namespace PBManager.DAL.EntityConfigurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            HasKey(e => e.Id);
        }
    }
}