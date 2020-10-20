using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBManager.Core.Models;

namespace PBManager.DAL.EntityConfigurations
{
    class CashflowConfiguration : EntityTypeConfiguration<Cashflow>
    {
        public CashflowConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.Description)
                // .IsRequired()
                .HasMaxLength(50);

            Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);

            Property(u => u.AccountingDate)
                .IsRequired();

            HasRequired(a => a.Account)
                .WithMany(b => b.Cashflows)
                .HasForeignKey(a => a.AccountId);

            HasOptional(a => a.Category)
                .WithMany(c => c.Cashflows)
                .HasForeignKey(a => a.CategoryId);

            HasOptional(a => a.Subcategory)
                .WithMany(c => c.Cashflows)
                .HasForeignKey(a => a.SubcategoryId);

            HasOptional(a => a.Project)
                .WithMany(c => c.Cashflows)
                .HasForeignKey(a => a.ProjectId);

        }
    }
}
