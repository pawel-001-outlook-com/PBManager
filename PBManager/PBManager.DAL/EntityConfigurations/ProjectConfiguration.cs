using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PBManager.Core.Models;

namespace PBManager.DAL.EntityConfigurations
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(30);

            Property(a => a.StartDate)
                .IsOptional();

            Property(a => a.FinishDate)
                .IsOptional();

            Property(a => a.Budget)
                .IsOptional();

            Property(a => a.Description)
                .IsOptional()
                .IsMaxLength();

            HasRequired(a => a.User)
                .WithMany(b => b.Projects)
                .HasForeignKey(a => a.UserId);
        }
    }
}