namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteImages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserBasicModels", "ProfileImage_ImageId", "dbo.ImageModels");
            DropIndex("dbo.UserBasicModels", new[] { "ProfileImage_ImageId" });
            DropColumn("dbo.UserBasicModels", "ProfileImage_ImageId");
            DropTable("dbo.ImageModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ImageModels",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.ImageId);
            
            AddColumn("dbo.UserBasicModels", "ProfileImage_ImageId", c => c.Int());
            CreateIndex("dbo.UserBasicModels", "ProfileImage_ImageId");
            AddForeignKey("dbo.UserBasicModels", "ProfileImage_ImageId", "dbo.ImageModels", "ImageId");
        }
    }
}
