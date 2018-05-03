namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeCollectionsToStringWithCommas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectModels", "UsersBelongsToProject", c => c.String());
            AddColumn("dbo.ProjectModels", "SkillsRequiredToProject", c => c.String());
            AddColumn("dbo.OfferToProjectModels", "WorkersProposedToProject", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OfferToProjectModels", "WorkersProposedToProject");
            DropColumn("dbo.ProjectModels", "SkillsRequiredToProject");
            DropColumn("dbo.ProjectModels", "UsersBelongsToProject");
        }
    }
}
