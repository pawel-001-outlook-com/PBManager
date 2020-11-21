using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PBManager.Core.Models;

namespace PBManager.DAL.EntityConfigurations
{
    public class SubcategoryConfiguration : EntityTypeConfiguration<Subcategory>
    {
        public SubcategoryConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(30);

            HasMany(a => a.Cashflows);


            HasRequired(a => a.Category)
                .WithMany(b => b.Subcategories)
                .HasForeignKey(a => a.CategoryId);
        }
    }
}