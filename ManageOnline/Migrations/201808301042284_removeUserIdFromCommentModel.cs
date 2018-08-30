namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUserIdFromCommentModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommentModels", "UserId", "dbo.UserBasicModels");
            DropIndex("dbo.CommentModels", new[] { "UserId" });
            RenameColumn(table: "dbo.CommentModels", name: "UserId", newName: "UserWhoAddComment_UserId");
            AlterColumn("dbo.CommentModels", "UserWhoAddComment_UserId", c => c.Int());
            CreateIndex("dbo.CommentModels", "UserWhoAddComment_UserId");
            AddForeignKey("dbo.CommentModels", "UserWhoAddComment_UserId", "dbo.UserBasicModels", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentModels", "UserWhoAddComment_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.CommentModels", new[] { "UserWhoAddComment_UserId" });
            AlterColumn("dbo.CommentModels", "UserWhoAddComment_UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.CommentModels", name: "UserWhoAddComment_UserId", newName: "UserId");
            CreateIndex("dbo.CommentModels", "UserId");
            AddForeignKey("dbo.CommentModels", "UserId", "dbo.UserBasicModels", "UserId", cascadeDelete: true);
        }
    }
}
