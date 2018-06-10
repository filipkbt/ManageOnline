namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotificationTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotificationModels", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotificationModels", "Role");
        }
    }
}
