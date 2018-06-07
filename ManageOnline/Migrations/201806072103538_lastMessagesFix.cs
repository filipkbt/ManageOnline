namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lastMessagesFix : DbMigration
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
        Content = c.String(nullable: false),
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
        }
    }
}
