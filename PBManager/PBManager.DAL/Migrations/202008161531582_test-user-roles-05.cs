namespace PBManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testuserroles05 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserRoles", newName: "UserHasManyRoles");
            RenameColumn(table: "dbo.UserHasManyRoles", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.UserHasManyRoles", name: "Role_Id", newName: "RoleId");
            RenameIndex(table: "dbo.UserHasManyRoles", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.UserHasManyRoles", name: "IX_Role_Id", newName: "IX_RoleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UserHasManyRoles", name: "IX_RoleId", newName: "IX_Role_Id");
            RenameIndex(table: "dbo.UserHasManyRoles", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.UserHasManyRoles", name: "RoleId", newName: "Role_Id");
            RenameColumn(table: "dbo.UserHasManyRoles", name: "UserId", newName: "User_Id");
            RenameTable(name: "dbo.UserHasManyRoles", newName: "UserRoles");
        }
    }
}
