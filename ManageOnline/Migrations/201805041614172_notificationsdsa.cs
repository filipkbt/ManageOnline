namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notificationsdsa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationModels",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        IsSeen = c.Boolean(nullable: false),
                        DateSend = c.DateTime(nullable: false),
                        Content = c.String(),
                        Receiver_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.UserBasicModels", t => t.Receiver_UserId)
                .Index(t => t.Receiver_UserId);
            
            AlterColumn("dbo.MessageModels", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotificationModels", "Receiver_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.NotificationModels", new[] { "Receiver_UserId" });
            AlterColumn("dbo.MessageModels", "Content", c => c.Int(nullable: false));
            DropTable("dbo.NotificationModels");
        }
    }
}
