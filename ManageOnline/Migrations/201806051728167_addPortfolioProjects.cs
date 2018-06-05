namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPortfolioProjects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasicModels", "PortfolioProjects", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasicModels", "PortfolioProjects");
        }
    }
}
