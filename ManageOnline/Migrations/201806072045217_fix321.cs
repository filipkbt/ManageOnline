namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix321 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.NotificationModels", name: "Receiver_UserId", newName: "NotificationReceiver_UserId");
            RenameIndex(table: "dbo.NotificationModels", name: "IX_Receiver_UserId", newName: "IX_NotificationReceiver_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.NotificationModels", name: "IX_NotificationReceiver_UserId", newName: "IX_Receiver_UserId");
            RenameColumn(table: "dbo.NotificationModels", name: "NotificationReceiver_UserId", newName: "Receiver_UserId");
        }
    }
}
