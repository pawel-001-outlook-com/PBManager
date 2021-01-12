namespace PBManager.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class accountdeleteoncascadecashflow : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Cashflows", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
        }
    }
}
