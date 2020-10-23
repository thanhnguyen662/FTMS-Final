namespace FTMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainerToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManageTrainers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrainerId = c.String(maxLength: 128),
                        TopicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerId)
                .Index(t => t.TrainerId)
                .Index(t => t.TopicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ManageTrainers", "TrainerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ManageTrainers", "TopicId", "dbo.Topics");
            DropIndex("dbo.ManageTrainers", new[] { "TopicId" });
            DropIndex("dbo.ManageTrainers", new[] { "TrainerId" });
            DropTable("dbo.ManageTrainers");
        }
    }
}
