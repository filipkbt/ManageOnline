namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class projectModelActualize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectModels", "ProjectStatus", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectModels", "ProjectBudget", c => c.String());
            DropColumn("dbo.ProjectModels", "IsProjectAllowedToSomeone");
            DropColumn("dbo.ProjectModels", "IsProjectInProgress");
            DropColumn("dbo.ProjectModels", "IsProjectFinished");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectModels", "IsProjectFinished", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProjectModels", "IsProjectInProgress", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProjectModels", "IsProjectAllowedToSomeone", c => c.Boolean(nullable: false));
            DropColumn("dbo.ProjectModels", "ProjectBudget");
            DropColumn("dbo.ProjectModels", "ProjectStatus");
        }
    }
}
