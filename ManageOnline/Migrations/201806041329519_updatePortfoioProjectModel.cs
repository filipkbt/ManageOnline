namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePortfoioProjectModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PortoflioProjectModels", "ProjectName", c => c.String());
            AddColumn("dbo.PortoflioProjectModels", "ProjectDescription", c => c.String());
            AddColumn("dbo.PortoflioProjectModels", "ProjectLink", c => c.String());
            AddColumn("dbo.PortoflioProjectModels", "ProjectImage", c => c.Binary());
            AddColumn("dbo.OfferToProjectModels", "WorkerProposedToProject", c => c.String());
            DropColumn("dbo.PortoflioProjectModels", "PortfolioProjectName");
            DropColumn("dbo.PortoflioProjectModels", "PortfolioProjectDescription");
            DropColumn("dbo.PortoflioProjectModels", "PortfolioProjectLink");
            DropColumn("dbo.OfferToProjectModels", "WorkersProposedToProject");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OfferToProjectModels", "WorkersProposedToProject", c => c.String());
            AddColumn("dbo.PortoflioProjectModels", "PortfolioProjectLink", c => c.String());
            AddColumn("dbo.PortoflioProjectModels", "PortfolioProjectDescription", c => c.String());
            AddColumn("dbo.PortoflioProjectModels", "PortfolioProjectName", c => c.String());
            DropColumn("dbo.OfferToProjectModels", "WorkerProposedToProject");
            DropColumn("dbo.PortoflioProjectModels", "ProjectImage");
            DropColumn("dbo.PortoflioProjectModels", "ProjectLink");
            DropColumn("dbo.PortoflioProjectModels", "ProjectDescription");
            DropColumn("dbo.PortoflioProjectModels", "ProjectName");
        }
    }
}
