namespace FTMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTrainerToDb1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ManageTrainers", "TrainerId", "dbo.AspNetUsers");
            DropIndex("dbo.ManageTrainers", new[] { "TrainerId" });
            AlterColumn("dbo.ManageTrainers", "TrainerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ManageTrainers", "TrainerId");
            AddForeignKey("dbo.ManageTrainers", "TrainerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ManageTrainers", "TrainerId", "dbo.AspNetUsers");
            DropIndex("dbo.ManageTrainers", new[] { "TrainerId" });
            AlterColumn("dbo.ManageTrainers", "TrainerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ManageTrainers", "TrainerId");
            AddForeignKey("dbo.ManageTrainers", "TrainerId", "dbo.AspNetUsers", "Id");
        }
    }
}
