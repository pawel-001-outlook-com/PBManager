using PBManager.Core.Models;
using PBManager.DAL.EntityConfigurations;
using System.Data.Entity;

namespace PBManager.DAL
{
    public class DataContext : DbContext
    {
        public DataContext()
        // : base("Data Source=localhost\\sqlexpress2014;Initial Catalog= pbm030 ;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False") { }
        : base("Data Source=db-mssql;Initial Catalog=s16307  ;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False") { }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Cashflow> Cashflows { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new AccountConfiguration());
            modelBuilder.Configurations.Add(new CashflowConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new SubcategoryConfiguration());
            modelBuilder.Configurations.Add(new ProjectConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());

            // modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
