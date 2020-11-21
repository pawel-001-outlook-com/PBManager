using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PBManager.Core.Models;

namespace PBManager.DAL.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasKey(t => t.Id);

            Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(30);

            Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(30);

            Property(u => u.Password)
                .IsRequired();

            HasMany(e => e.Roles)
                .WithMany(t => t.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("RoleId");
                    cs.ToTable("UsersRoles");
                });
        }
    }
}