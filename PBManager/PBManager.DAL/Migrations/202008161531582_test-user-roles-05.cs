using System.Data.Entity.Migrations;

namespace PBManager.DAL.Migrations
{
    public partial class testuserroles05 : DbMigration
    {
        public override void Up()
        {
            RenameTable("dbo.UserRoles", "UserHasManyRoles");
            RenameColumn("dbo.UserHasManyRoles", "User_Id", "UserId");
            RenameColumn("dbo.UserHasManyRoles", "Role_Id", "RoleId");
            RenameIndex("dbo.UserHasManyRoles", "IX_User_Id", "IX_UserId");
            RenameIndex("dbo.UserHasManyRoles", "IX_Role_Id", "IX_RoleId");
        }

        public override void Down()
        {
            RenameIndex("dbo.UserHasManyRoles", "IX_RoleId", "IX_Role_Id");
            RenameIndex("dbo.UserHasManyRoles", "IX_UserId", "IX_User_Id");
            RenameColumn("dbo.UserHasManyRoles", "RoleId", "Role_Id");
            RenameColumn("dbo.UserHasManyRoles", "UserId", "User_Id");
            RenameTable("dbo.UserHasManyRoles", "UserRoles");
        }
    }
}