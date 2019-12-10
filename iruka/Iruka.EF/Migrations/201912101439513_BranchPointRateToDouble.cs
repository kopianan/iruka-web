namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BranchPointRateToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Branches", "PointRate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Branches", "PointRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
