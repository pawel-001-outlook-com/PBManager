using PBManager.Core.Models;
using System.Data.Entity.ModelConfiguration;

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