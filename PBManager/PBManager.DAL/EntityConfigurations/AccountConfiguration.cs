using PBManager.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

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

            Property(u => u.InitialBalance)
                .IsRequired();

            Property(a => a.Balance)
                .IsOptional();

            HasMany(a => a.Cashflows)
                .WithRequired(c => c.Account)
                .HasForeignKey(c => c.AccountId)
                .WillCascadeOnDelete();

            HasRequired(a => a.User)
                .WithMany(b => b.Accounts)
                .HasForeignKey(a => a.UserId);

            Property(a => a.Description)
                .IsOptional();

        }
    }
}
