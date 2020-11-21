using System.Data.Entity.Migrations;

namespace PBManager.DAL.Migrations
{
    public partial class addmanyusersroles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Roles",
                    c => new
                    {
                        Id = c.Int(false, true),
                        RoleName = c.String()
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.RoleUsers",
                    c => new
                    {
                        Role_Id = c.Int(false),
                        User_Id = c.Int(false)
                    })
                .PrimaryKey(t => new {t.Role_Id, t.User_Id})
                .ForeignKey("dbo.Roles", t => t.Role_Id, true)
                .ForeignKey("dbo.Users", t => t.User_Id, true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropIndex("dbo.RoleUsers", new[] {"User_Id"});
            DropIndex("dbo.RoleUsers", new[] {"Role_Id"});
            DropTable("dbo.RoleUsers");
            DropTable("dbo.Roles");
        }
    }
}