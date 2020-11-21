using System.Data.Entity.Migrations;

namespace PBManager.DAL.Migrations
{
    public partial class test010 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Accounts",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 30),
                        InitialBalance = c.Double(false),
                        Balance = c.Double(),
                        UserId = c.Int(false),
                        Description = c.String()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, true)
                .Index(t => t.UserId);

            CreateTable(
                    "dbo.Cashflows",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 50),
                        Description = c.String(maxLength: 50),
                        Value = c.Double(false),
                        AccountingDate = c.DateTime(false),
                        AccountId = c.Int(false),
                        CategoryId = c.Int(),
                        SubcategoryId = c.Int(),
                        ProjectId = c.Int()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.Subcategories", t => t.SubcategoryId)
                .ForeignKey("dbo.Accounts", t => t.AccountId, true)
                .Index(t => t.AccountId)
                .Index(t => t.CategoryId)
                .Index(t => t.SubcategoryId)
                .Index(t => t.ProjectId);

            CreateTable(
                    "dbo.Categories",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 30),
                        Type = c.String(),
                        UserID = c.Int(false),
                        Description = c.String()
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserID, true)
                .Index(t => t.UserID);

            CreateTable(
                    "dbo.Subcategories",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 30),
                        CategoryId = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, true)
                .Index(t => t.CategoryId);

            CreateTable(
                    "dbo.Users",
                    c => new
                    {
                        Id = c.Int(false, true),
                        UserName = c.String(false, 30),
                        FirstName = c.String(),
                        Surname = c.String(),
                        Password = c.String(false),
                        Email = c.String(false, 30)
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.Projects",
                    c => new
                    {
                        Id = c.Int(false, true),
                        Name = c.String(false, 30),
                        StartDate = c.DateTime(),
                        FinishDate = c.DateTime(),
                        Budget = c.Double(),
                        Description = c.String(),
                        UserId = c.Int(false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, true)
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Cashflows", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Cashflows", "SubcategoryId", "dbo.Subcategories");
            DropForeignKey("dbo.Cashflows", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Cashflows", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "UserID", "dbo.Users");
            DropForeignKey("dbo.Projects", "UserId", "dbo.Users");
            DropForeignKey("dbo.Subcategories", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Projects", new[] {"UserId"});
            DropIndex("dbo.Subcategories", new[] {"CategoryId"});
            DropIndex("dbo.Categories", new[] {"UserID"});
            DropIndex("dbo.Cashflows", new[] {"ProjectId"});
            DropIndex("dbo.Cashflows", new[] {"SubcategoryId"});
            DropIndex("dbo.Cashflows", new[] {"CategoryId"});
            DropIndex("dbo.Cashflows", new[] {"AccountId"});
            DropIndex("dbo.Accounts", new[] {"UserId"});
            DropTable("dbo.Projects");
            DropTable("dbo.Users");
            DropTable("dbo.Subcategories");
            DropTable("dbo.Categories");
            DropTable("dbo.Cashflows");
            DropTable("dbo.Accounts");
        }
    }
}