namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCategoriesModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoriesModels",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            AddColumn("dbo.ProjectModels", "ProjectCategory_CategoryId", c => c.Int());
            AlterColumn("dbo.SkillsModels", "SkillName", c => c.String());
            CreateIndex("dbo.ProjectModels", "ProjectCategory_CategoryId");
            AddForeignKey("dbo.ProjectModels", "ProjectCategory_CategoryId", "dbo.CategoriesModels", "CategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectModels", "ProjectCategory_CategoryId", "dbo.CategoriesModels");
            DropIndex("dbo.ProjectModels", new[] { "ProjectCategory_CategoryId" });
            AlterColumn("dbo.SkillsModels", "SkillName", c => c.Int(nullable: false));
            DropColumn("dbo.ProjectModels", "ProjectCategory_CategoryId");
            DropTable("dbo.CategoriesModels");
        }
    }
}
