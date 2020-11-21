using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PBManager.Core.Models;

namespace PBManager.DAL.EntityConfigurations
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(30);

            HasMany(a => a.Cashflows);

            HasMany(a => a.Subcategories);


            HasRequired(a => a.User)
                .WithMany(b => b.Categories)
                .HasForeignKey(a => a.UserID);
        }
    }
}