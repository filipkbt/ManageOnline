namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTitleToNotificationModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotificationModels", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NotificationModels", "Title");
        }
    }
}
