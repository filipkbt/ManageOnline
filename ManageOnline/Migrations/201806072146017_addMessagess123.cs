namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMessagess123 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessagesModels",
                c => new
                {
                    MessageId = c.Int(nullable: false, identity: true),
                    IsSeen = c.Boolean(nullable: false),
                    DateSend = c.DateTime(nullable: false),
                    Title = c.String(),
                    Content = c.String(),
                    Receiver_UserId = c.Int(),
                    Sender_UserId = c.Int(),
                })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.UserBasicModels", t => t.Receiver_UserId)
                .ForeignKey("dbo.UserBasicModels", t => t.Sender_UserId)
                .Index(t => t.Receiver_UserId)
                .Index(t => t.Sender_UserId);            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessagesModels", "UserBasicModel_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.MessagesModels", "Sender_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.MessagesModels", "Receiver_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.MessagesModels", new[] { "UserBasicModel_UserId" });
            DropIndex("dbo.MessagesModels", new[] { "Sender_UserId" });
            DropIndex("dbo.MessagesModels", new[] { "Receiver_UserId" });
            DropTable("dbo.MessagesModels");
        }
    }
}
