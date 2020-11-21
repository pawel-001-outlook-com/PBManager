using System.Data.Entity.Migrations;

namespace PBManager.DAL.Migrations
{
    public partial class testserroles04 : DbMigration
    {
        public override void Up()
        {
            RenameTable("dbo.RoleUsers", "UserRoles");
            DropPrimaryKey("dbo.UserRoles");
            AddPrimaryKey("dbo.UserRoles", new[] {"User_Id", "Role_Id"});
        }

        public override void Down()
        {
            DropPrimaryKey("dbo.UserRoles");
            AddPrimaryKey("dbo.UserRoles", new[] {"Role_Id", "User_Id"});
            RenameTable("dbo.UserRoles", "RoleUsers");
        }
    }
}