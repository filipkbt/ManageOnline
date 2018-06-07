namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMessagesaeae1 : DbMigration
    {
        public override void Up()
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
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.UserBasicModels", t => t.Receiver_UserId)
                .Index(t => t.Receiver_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageModels", "Receiver_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.MessageModels", new[] { "Receiver_UserId" });
            DropTable("dbo.MessageModels");
        }
    }
}
