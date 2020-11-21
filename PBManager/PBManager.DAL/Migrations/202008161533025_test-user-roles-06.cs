using System.Data.Entity.Migrations;

namespace PBManager.DAL.Migrations
{
    public partial class testuserroles06 : DbMigration
    {
        public override void Up()
        {
            RenameTable("dbo.UserHasManyRoles", "UserRole");
        }

        public override void Down()
        {
            RenameTable("dbo.UserRole", "UserHasManyRoles");
        }
    }
}