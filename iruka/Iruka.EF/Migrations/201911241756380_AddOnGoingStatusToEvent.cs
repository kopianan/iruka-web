namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOnGoingStatusToEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "OnGoing", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "OnGoing");
        }
    }
}
