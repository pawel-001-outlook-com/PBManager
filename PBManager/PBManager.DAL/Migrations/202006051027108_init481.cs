namespace PBManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init481 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        InitialBalance = c.Double(nullable: false),
                        Balance = c.Double(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                        UserID = c.Int(nullable: false),
                        AccountKind_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountKinds", t => t.AccountKind_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.AccountKind_Id);
            
            CreateTable(
                "dbo.AccountKinds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cashflows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 1),
                        Description = c.String(nullable: false, maxLength: 50),
                        Value = c.Double(nullable: false),
                        InclusionDate = c.DateTime(nullable: false),
                        AccountingDate = c.DateTime(nullable: false),
                        AccountId = c.Int(nullable: false),
                        SubcategoryId = c.Int(nullable: false),
                        ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.Subcategories", t => t.SubcategoryId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.SubcategoryId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        StartDate = c.DateTime(nullable: false),
                        FinishDate = c.DateTime(),
                        Budget = c.Double(),
                        Description = c.String(maxLength: 100),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subcategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        Password = c.String(),
                        Initials = c.String(),
                        Email = c.String(),
                        Phone = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubcategoryBudgets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubcategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subcategories", t => t.SubcategoryID, cascadeDelete: true)
                .Index(t => t.SubcategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubcategoryBudgets", "SubcategoryID", "dbo.Subcategories");
            DropForeignKey("dbo.Accounts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Cashflows", "SubcategoryId", "dbo.Subcategories");
            DropForeignKey("dbo.Subcategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Cashflows", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Cashflows", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "AccountKind_Id", "dbo.AccountKinds");
            DropIndex("dbo.SubcategoryBudgets", new[] { "SubcategoryID" });
            DropIndex("dbo.Subcategories", new[] { "CategoryId" });
            DropIndex("dbo.Cashflows", new[] { "ProjectId" });
            DropIndex("dbo.Cashflows", new[] { "SubcategoryId" });
            DropIndex("dbo.Cashflows", new[] { "AccountId" });
            DropIndex("dbo.Accounts", new[] { "AccountKind_Id" });
            DropIndex("dbo.Accounts", new[] { "UserID" });
            DropTable("dbo.SubcategoryBudgets");
            DropTable("dbo.Users");
            DropTable("dbo.Categories");
            DropTable("dbo.Subcategories");
            DropTable("dbo.Projects");
            DropTable("dbo.Cashflows");
            DropTable("dbo.AccountKinds");
            DropTable("dbo.Accounts");
        }
    }
}
