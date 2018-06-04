namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IfProjectNeedManager : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectModels", "IsRequiredManager", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectModels", "IsRequiredManager");
        }
    }
}
