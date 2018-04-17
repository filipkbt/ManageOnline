namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOfferToProjectModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OfferToProjectModels",
                c => new
                    {
                        OfferToProjectId = c.Int(nullable: false, identity: true),
                        AddOfferDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Budget = c.Double(nullable: false),
                        EstimatedTimeToFinishProject = c.Int(nullable: false),
                        ProjectWhereOfferWasAdded_ProjectId = c.Int(),
                        UserWhoAddOffer_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.OfferToProjectId)
                .ForeignKey("dbo.ProjectModels", t => t.ProjectWhereOfferWasAdded_ProjectId)
                .ForeignKey("dbo.UserBasicModels", t => t.UserWhoAddOffer_UserId)
                .Index(t => t.ProjectWhereOfferWasAdded_ProjectId)
                .Index(t => t.UserWhoAddOffer_UserId);
            
            AddColumn("dbo.UserBasicModels", "OfferToProjectModel_OfferToProjectId", c => c.Int());
            CreateIndex("dbo.UserBasicModels", "OfferToProjectModel_OfferToProjectId");
            AddForeignKey("dbo.UserBasicModels", "OfferToProjectModel_OfferToProjectId", "dbo.OfferToProjectModels", "OfferToProjectId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserBasicModels", "OfferToProjectModel_OfferToProjectId", "dbo.OfferToProjectModels");
            DropForeignKey("dbo.OfferToProjectModels", "UserWhoAddOffer_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.OfferToProjectModels", "ProjectWhereOfferWasAdded_ProjectId", "dbo.ProjectModels");
            DropIndex("dbo.OfferToProjectModels", new[] { "UserWhoAddOffer_UserId" });
            DropIndex("dbo.OfferToProjectModels", new[] { "ProjectWhereOfferWasAdded_ProjectId" });
            DropIndex("dbo.UserBasicModels", new[] { "OfferToProjectModel_OfferToProjectId" });
            DropColumn("dbo.UserBasicModels", "OfferToProjectModel_OfferToProjectId");
            DropTable("dbo.OfferToProjectModels");
        }
    }
}
