namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventPriorityToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "Priority", c => c.Int(nullable: false));
        }
    }
}
