using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;

namespace PBManager.DAL.EntityConfigurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            HasKey(e => e.Id);


            // HasMany<User>(e => e.Users)
            //     .WithMany(c => c.Roles);
            // .Map(cs =>
            // {
            //     cs.MapLeftKey("Id");
            //     cs.MapRightKey("Id");
            //     cs.ToTable("UserRole");
            // });
        }
        

    }

}