namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class littleFixNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotificationModels", "NotificationType", c => c.Int(nullable: false));
            DropColumn("dbo.NotificationModels", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NotificationModels", "Role", c => c.Int(nullable: false));
            DropColumn("dbo.NotificationModels", "NotificationType");
        }
    }
}
