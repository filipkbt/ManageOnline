namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameColumnCategoriesAtProjectModels : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ProjectModels", name: "CategoriesModel_CategoryId", newName: "ProjectCategory_CategoryId");
            RenameIndex(table: "dbo.ProjectModels", name: "IX_CategoriesModel_CategoryId", newName: "IX_ProjectCategory_CategoryId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ProjectModels", name: "IX_ProjectCategory_CategoryId", newName: "IX_CategoriesModel_CategoryId");
            RenameColumn(table: "dbo.ProjectModels", name: "ProjectCategory_CategoryId", newName: "CategoriesModel_CategoryId");
        }
    }
}
