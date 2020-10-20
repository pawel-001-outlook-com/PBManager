namespace PBManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testuserroles06 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserHasManyRoles", newName: "UserRole");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.UserRole", newName: "UserHasManyRoles");
        }
    }
}
