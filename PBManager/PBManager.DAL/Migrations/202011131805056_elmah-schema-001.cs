namespace PBManager.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class elmahschema001 : DbMigration
    {
        public override void Up()
        {
            SqlFile("E:/_temp_pj/PBManager 20201113_1909_errorhandling_elmah/PBManager/PBManager.DAL/Migrations/ELMAH-1.2-db-SQLServer.sql");
        }
        
        public override void Down()
        {
        }
    }
}
