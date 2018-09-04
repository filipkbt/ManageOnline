namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsFinishedToScrumSprintModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScrumSprintModels", "IsFinished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScrumSprintModels", "IsFinished");
        }
    }
}
