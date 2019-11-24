namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OnGoingChangedToEnumEventStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Events", "OnGoing");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "OnGoing", c => c.Boolean(nullable: false));
            DropColumn("dbo.Events", "EventStatus");
        }
    }
}
