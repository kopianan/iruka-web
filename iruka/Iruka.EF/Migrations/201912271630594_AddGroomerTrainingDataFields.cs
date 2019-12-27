namespace Iruka.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroomerTrainingDataFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TrainingStartDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "TrainingYears", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "TrainingCourses", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TrainingCourses");
            DropColumn("dbo.AspNetUsers", "TrainingYears");
            DropColumn("dbo.AspNetUsers", "TrainingStartDate");
        }
    }
}
