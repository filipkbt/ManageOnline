namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserBasicModels", "UserBasicModel_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.ProjectModels", "UserBasicModel_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.UserBasicModels", new[] { "UserBasicModel_UserId" });
            DropIndex("dbo.ProjectModels", new[] { "UserBasicModel_UserId" });
            DropColumn("dbo.UserBasicModels", "ConfirmPassword");
            DropColumn("dbo.UserBasicModels", "PortfolioProjects");
            DropColumn("dbo.UserBasicModels", "UserBasicModel_UserId");
            DropColumn("dbo.ProjectModels", "UserBasicModel_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectModels", "UserBasicModel_UserId", c => c.Int());
            AddColumn("dbo.UserBasicModels", "UserBasicModel_UserId", c => c.Int());
            AddColumn("dbo.UserBasicModels", "PortfolioProjects", c => c.String());
            AddColumn("dbo.UserBasicModels", "ConfirmPassword", c => c.String());
            CreateIndex("dbo.ProjectModels", "UserBasicModel_UserId");
            CreateIndex("dbo.UserBasicModels", "UserBasicModel_UserId");
            AddForeignKey("dbo.ProjectModels", "UserBasicModel_UserId", "dbo.UserBasicModels", "UserId");
            AddForeignKey("dbo.UserBasicModels", "UserBasicModel_UserId", "dbo.UserBasicModels", "UserId");
        }
    }
}
