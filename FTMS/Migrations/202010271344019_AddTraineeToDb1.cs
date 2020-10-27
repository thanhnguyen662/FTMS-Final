namespace FTMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTraineeToDb1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ManageTrainees", "TraineeId", "dbo.AspNetUsers");
            DropIndex("dbo.ManageTrainees", new[] { "TraineeId" });
            AlterColumn("dbo.ManageTrainees", "TraineeId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ManageTrainees", "TraineeId");
            AddForeignKey("dbo.ManageTrainees", "TraineeId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ManageTrainees", "TraineeId", "dbo.AspNetUsers");
            DropIndex("dbo.ManageTrainees", new[] { "TraineeId" });
            AlterColumn("dbo.ManageTrainees", "TraineeId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ManageTrainees", "TraineeId");
            AddForeignKey("dbo.ManageTrainees", "TraineeId", "dbo.AspNetUsers", "Id");
        }
    }
}
