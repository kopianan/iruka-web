namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_picture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Picture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Picture");
        }
    }
}
