namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScrumSprint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScrumSprintModels", "ScrumSprintNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScrumSprintModels", "ScrumSprintNumber");
        }
    }
}
