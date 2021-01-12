namespace PBManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testserroles04 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RoleUsers", newName: "UserRoles");
            DropPrimaryKey("dbo.UserRoles");
            AddPrimaryKey("dbo.UserRoles", new[] { "User_Id", "Role_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.UserRoles");
            AddPrimaryKey("dbo.UserRoles", new[] { "Role_Id", "User_Id" });
            RenameTable(name: "dbo.UserRoles", newName: "RoleUsers");
        }
    }
}
