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
    class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration()
        {
            HasKey(b => b.Id);

            Property(b => b.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(30);

            // HasOptional(a => a.AccountKind)
            //     // .WithMany(k => k.Accounts)
            //     .WithMany()
            //     .HasForeignKey(a => a.AccountKindId);

            Property(u => u.InitialBalance)
                .IsRequired();

            Property(a => a.Balance)
                .IsOptional();

            HasMany(a => a.Cashflows)
                .WithRequired(c => c.Account)
                .HasForeignKey(c => c.AccountId);

            HasRequired(a => a.User)
                .WithMany(b => b.Accounts)
                .HasForeignKey(a => a.UserId);

            Property(a => a.Description)
                .IsOptional();

        }
    }
}
