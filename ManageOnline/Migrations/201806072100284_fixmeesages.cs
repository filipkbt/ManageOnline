namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixmeesages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MessageModels", "Sender_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.MessageModels", "Receiver_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.MessageModels", new[] { "Sender_UserId" });
            DropIndex("dbo.MessageModels", new[] { "Receiver_UserId" });
            DropTable("dbo.MessageModels");
        }
        
        public override void Down()
        {
        }
    }
}
