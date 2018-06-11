namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVariablesToUserBasicModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBasicModels", "FinishedProjects", c => c.Int(nullable: false));
            AddColumn("dbo.UserBasicModels", "ProjectsInProgress", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBasicModels", "ProjectsInProgress");
            DropColumn("dbo.UserBasicModels", "FinishedProjects");
        }
    }
}
