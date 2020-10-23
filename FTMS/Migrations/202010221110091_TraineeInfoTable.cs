namespace FTMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TraineeInfoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TraineeInfoes",
                c => new
                    {
                        TraineeInfoId = c.Int(nullable: false, identity: true),
                        TraineeId = c.String(nullable: false, maxLength: 128),
                        Full_Name = c.String(),
                        Education = c.String(),
                        Programming_Language = c.String(),
                        Age = c.Int(nullable: false),
                        Experience_Details = c.String(),
                        Location = c.String(),
                        Toeic_Score = c.Int(nullable: false),
                        Department = c.String(),
                    })
                .PrimaryKey(t => t.TraineeInfoId)
                .ForeignKey("dbo.AspNetUsers", t => t.TraineeId, cascadeDelete: true)
                .Index(t => t.TraineeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TraineeInfoes", "TraineeId", "dbo.AspNetUsers");
            DropIndex("dbo.TraineeInfoes", new[] { "TraineeId" });
            DropTable("dbo.TraineeInfoes");
        }
    }
}
