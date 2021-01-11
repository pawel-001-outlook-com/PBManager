namespace PBManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test010 : DbMigration
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
                        Balance = c.Double(),
                        UserId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Cashflows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 50),
                        Value = c.Double(nullable: false),
                        AccountingDate = c.DateTime(nullable: false),
                        AccountId = c.Int(nullable: false),
                        CategoryId = c.Int(),
                        SubcategoryId = c.Int(),
                        ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .ForeignKey("dbo.Subcategories", t => t.SubcategoryId)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.CategoryId)
                .Index(t => t.SubcategoryId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Type = c.String(),
                        UserID = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Subcategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 30),
                        FirstName = c.String(),
                        Surname = c.String(),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        StartDate = c.DateTime(),
                        FinishDate = c.DateTime(),
                        Budget = c.Double(),
                        Description = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
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
            DropIndex("dbo.Projects", new[] { "UserId" });
            DropIndex("dbo.Subcategories", new[] { "CategoryId" });
            DropIndex("dbo.Categories", new[] { "UserID" });
            DropIndex("dbo.Cashflows", new[] { "ProjectId" });
            DropIndex("dbo.Cashflows", new[] { "SubcategoryId" });
            DropIndex("dbo.Cashflows", new[] { "CategoryId" });
            DropIndex("dbo.Cashflows", new[] { "AccountId" });
            DropIndex("dbo.Accounts", new[] { "UserId" });
            DropTable("dbo.Projects");
            DropTable("dbo.Users");
            DropTable("dbo.Subcategories");
            DropTable("dbo.Categories");
            DropTable("dbo.Cashflows");
            DropTable("dbo.Accounts");
        }
    }
}
