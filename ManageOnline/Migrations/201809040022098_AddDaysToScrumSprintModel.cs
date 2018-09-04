namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDaysToScrumSprintModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScrumSprintModels", "ScrumSprintLengthInDays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScrumSprintModels", "ScrumSprintLengthInDays");
        }
    }
}
