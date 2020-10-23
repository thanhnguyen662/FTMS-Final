namespace FTMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrainerInfoTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainerInfoes",
                c => new
                    {
                        TrainerInfoId = c.Int(nullable: false, identity: true),
                        TrainerId = c.String(nullable: false, maxLength: 128),
                        Full_Name = c.String(),
                        Email = c.String(),
                        Working_Place = c.String(),
                        Phone = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TrainerInfoId)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.TrainerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainerInfoes", "TrainerId", "dbo.AspNetUsers");
            DropIndex("dbo.TrainerInfoes", new[] { "TrainerId" });
            DropTable("dbo.TrainerInfoes");
        }
    }
}
