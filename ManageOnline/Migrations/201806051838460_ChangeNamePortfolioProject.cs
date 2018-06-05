namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNamePortfolioProject : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PortoflioProjectModels", newName: "PortfolioProjectModels");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PortfolioProjectModels", newName: "PortoflioProjectModels");
        }
    }
}
