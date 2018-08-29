namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentWhereCommentBelongToCommentModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentModels", "CommentConnectedWithSelectedComment_CommentId", c => c.Int());
            CreateIndex("dbo.CommentModels", "CommentConnectedWithSelectedComment_CommentId");
            AddForeignKey("dbo.CommentModels", "CommentConnectedWithSelectedComment_CommentId", "dbo.CommentModels", "CommentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentModels", "CommentConnectedWithSelectedComment_CommentId", "dbo.CommentModels");
            DropIndex("dbo.CommentModels", new[] { "CommentConnectedWithSelectedComment_CommentId" });
            DropColumn("dbo.CommentModels", "CommentConnectedWithSelectedComment_CommentId");
        }
    }
}
