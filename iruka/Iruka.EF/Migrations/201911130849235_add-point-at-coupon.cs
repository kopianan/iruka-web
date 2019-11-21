namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpointatcoupon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coupons", "Point", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Coupons", "Point");
        }
    }
}
