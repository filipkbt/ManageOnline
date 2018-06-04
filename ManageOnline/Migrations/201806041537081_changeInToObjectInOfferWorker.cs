namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeInToObjectInOfferWorker : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfferToProjectModels", "WorkerProposedToProject_UserId", c => c.Int());
            CreateIndex("dbo.OfferToProjectModels", "WorkerProposedToProject_UserId");
            AddForeignKey("dbo.OfferToProjectModels", "WorkerProposedToProject_UserId", "dbo.UserBasicModels", "UserId");
            DropColumn("dbo.OfferToProjectModels", "WorkerProposedToProject");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OfferToProjectModels", "WorkerProposedToProject", c => c.String());
            DropForeignKey("dbo.OfferToProjectModels", "WorkerProposedToProject_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.OfferToProjectModels", new[] { "WorkerProposedToProject_UserId" });
            DropColumn("dbo.OfferToProjectModels", "WorkerProposedToProject_UserId");
        }
    }
}
