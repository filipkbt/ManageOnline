namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MessageModels", "UserBasicModel_UserId1", "dbo.UserBasicModels");
            DropIndex("dbo.MessageModels", new[] { "UserBasicModel_UserId1" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.MessageModels", "UserBasicModel_UserId1");
            AddForeignKey("dbo.MessageModels", "UserBasicModel_UserId1", "dbo.UserBasicModels", "UserId");
        }
    }
}
