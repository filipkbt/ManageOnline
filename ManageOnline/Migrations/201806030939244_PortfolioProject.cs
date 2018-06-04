namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PortfolioProject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PortoflioProjectModels",
                c => new
                    {
                        PortfolioProjectId = c.Int(nullable: false, identity: true),
                        PortfolioProjectName = c.String(),
                        PortfolioProjectDescription = c.String(),
                        PortfolioProjectLink = c.String(),
                        EmployeeId_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.PortfolioProjectId)
                .ForeignKey("dbo.UserBasicModels", t => t.EmployeeId_UserId)
                .Index(t => t.EmployeeId_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PortoflioProjectModels", "EmployeeId_UserId", "dbo.UserBasicModels");
            DropIndex("dbo.PortoflioProjectModels", new[] { "EmployeeId_UserId" });
            DropTable("dbo.PortoflioProjectModels");
        }
    }
}
