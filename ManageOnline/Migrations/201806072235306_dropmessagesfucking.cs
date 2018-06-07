namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropmessagesfucking : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MessagesModels", "Receiver_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.MessagesModels", "Sender_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.MessagesModels", "UserBasicModel_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.MessagesModels", new[] { "Receiver_UserId" });
            DropIndex("dbo.MessagesModels", new[] { "Sender_UserId" });
            DropIndex("dbo.MessagesModels", new[] { "UserBasicModel_UserId" });
            DropTable("dbo.MessagesModels");
        }
        
        public override void Down()
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
                        UserBasicModel_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId);
            
            CreateIndex("dbo.MessagesModels", "UserBasicModel_UserId");
            CreateIndex("dbo.MessagesModels", "Sender_UserId");
            CreateIndex("dbo.MessagesModels", "Receiver_UserId");
            AddForeignKey("dbo.MessagesModels", "UserBasicModel_UserId", "dbo.UserBasicModels", "UserId");
            AddForeignKey("dbo.MessagesModels", "Sender_UserId", "dbo.UserBasicModels", "UserId");
            AddForeignKey("dbo.MessagesModels", "Receiver_UserId", "dbo.UserBasicModels", "UserId");
        }
    }
}
