namespace PBManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testuserroles07 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserRole", newName: "UsersRoles");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.UsersRoles", newName: "UserRole");
        }
    }
}
