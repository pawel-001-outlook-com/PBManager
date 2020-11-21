using System.Data.Entity.Migrations;

namespace PBManager.DAL.Migrations
{
    public partial class testuserroles07 : DbMigration
    {
        public override void Up()
        {
            RenameTable("dbo.UserRole", "UsersRoles");
        }

        public override void Down()
        {
            RenameTable("dbo.UsersRoles", "UserRole");
        }
    }
}