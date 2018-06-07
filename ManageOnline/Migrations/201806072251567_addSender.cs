namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessageModels", "Sender_UserId", c => c.Int());
            CreateIndex("dbo.MessageModels", "Sender_UserId");
            AddForeignKey("dbo.MessageModels", "Sender_UserId", "dbo.UserBasicModels", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageModels", "Sender_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.MessageModels", new[] { "Sender_UserId" });
            DropColumn("dbo.MessageModels", "Sender_UserId");
        }
    }
}
