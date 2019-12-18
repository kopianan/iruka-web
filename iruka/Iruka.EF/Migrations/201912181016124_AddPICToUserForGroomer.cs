namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPICToUserForGroomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PIC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PIC");
        }
    }
}
