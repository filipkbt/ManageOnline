namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminFixBug : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectModels", "ProjectCategory", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectModels", "ProjectCategory", c => c.String());
        }
    }
}
