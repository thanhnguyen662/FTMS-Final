namespace FTMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTraineeToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManageTrainees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TraineeId = c.String(maxLength: 128),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TraineeId)
                .Index(t => t.TraineeId)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ManageTrainees", "TraineeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ManageTrainees", "CourseId", "dbo.Courses");
            DropIndex("dbo.ManageTrainees", new[] { "CourseId" });
            DropIndex("dbo.ManageTrainees", new[] { "TraineeId" });
            DropTable("dbo.ManageTrainees");
        }
    }
}
