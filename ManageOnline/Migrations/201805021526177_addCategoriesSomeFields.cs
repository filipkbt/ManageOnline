namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCategoriesSomeFields : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ProjectModels", name: "ProjectCategory_CategoryId", newName: "CategoriesModel_CategoryId");
            RenameIndex(table: "dbo.ProjectModels", name: "IX_ProjectCategory_CategoryId", newName: "IX_CategoriesModel_CategoryId");
            AddColumn("dbo.ProjectModels", "ProjectCategory", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectModels", "ProjectCategory");
            RenameIndex(table: "dbo.ProjectModels", name: "IX_CategoriesModel_CategoryId", newName: "IX_ProjectCategory_CategoryId");
            RenameColumn(table: "dbo.ProjectModels", name: "CategoriesModel_CategoryId", newName: "ProjectCategory_CategoryId");
        }
    }
}
