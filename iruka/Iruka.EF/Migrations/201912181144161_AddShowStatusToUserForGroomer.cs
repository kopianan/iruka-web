namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShowStatusToUserForGroomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Show", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Show");
        }
    }
}
