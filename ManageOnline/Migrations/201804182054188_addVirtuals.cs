namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVirtuals : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OfferToProjectModels", "ProjectWhereOfferWasAdded_ProjectId", "dbo.ProjectModels");
            DropIndex("dbo.OfferToProjectModels", new[] { "ProjectWhereOfferWasAdded_ProjectId" });
            RenameColumn(table: "dbo.OfferToProjectModels", name: "ProjectWhereOfferWasAdded_ProjectId", newName: "ProjectId");
            AddColumn("dbo.TaskModels", "CurrentWorketAtTaskId", c => c.Int(nullable: false));
            AddColumn("dbo.OfferToProjectModels", "UserWhoAddOfferId", c => c.Int(nullable: false));
            AlterColumn("dbo.OfferToProjectModels", "ProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.OfferToProjectModels", "ProjectId");
            AddForeignKey("dbo.OfferToProjectModels", "ProjectId", "dbo.ProjectModels", "ProjectId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OfferToProjectModels", "ProjectId", "dbo.ProjectModels");
            DropIndex("dbo.OfferToProjectModels", new[] { "ProjectId" });
            AlterColumn("dbo.OfferToProjectModels", "ProjectId", c => c.Int());
            DropColumn("dbo.OfferToProjectModels", "UserWhoAddOfferId");
            DropColumn("dbo.TaskModels", "CurrentWorketAtTaskId");
            RenameColumn(table: "dbo.OfferToProjectModels", name: "ProjectId", newName: "ProjectWhereOfferWasAdded_ProjectId");
            CreateIndex("dbo.OfferToProjectModels", "ProjectWhereOfferWasAdded_ProjectId");
            AddForeignKey("dbo.OfferToProjectModels", "ProjectWhereOfferWasAdded_ProjectId", "dbo.ProjectModels", "ProjectId");
        }
    }
}
