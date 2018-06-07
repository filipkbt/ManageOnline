namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropMEssagesLast : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MessageModels", "Receiver_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.MessageModels", "Sender_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.MessageModels", "UserBasicModel_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.MessageModels", new[] { "Receiver_UserId" });
            DropIndex("dbo.MessageModels", new[] { "Sender_UserId" });
            DropIndex("dbo.MessageModels", new[] { "UserBasicModel_UserId" });
            DropTable("dbo.MessageModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MessageModels",
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
            
            CreateIndex("dbo.MessageModels", "UserBasicModel_UserId");
            CreateIndex("dbo.MessageModels", "Sender_UserId");
            CreateIndex("dbo.MessageModels", "Receiver_UserId");
            AddForeignKey("dbo.MessageModels", "UserBasicModel_UserId", "dbo.UserBasicModels", "UserId");
            AddForeignKey("dbo.MessageModels", "Sender_UserId", "dbo.UserBasicModels", "UserId");
            AddForeignKey("dbo.MessageModels", "Receiver_UserId", "dbo.UserBasicModels", "UserId");
        }
    }
}
