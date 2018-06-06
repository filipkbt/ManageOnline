namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RateModels",
                c => new
                    {
                        RateId = c.Int(nullable: false, identity: true),
                        Comment = c.String(nullable: false),
                        AverageRate = c.Int(nullable: false),
                        Communication = c.Int(nullable: false),
                        Professionalism = c.Int(nullable: false),
                        MeetingTheConditions = c.Int(nullable: false),
                        Skills = c.Int(),
                        Punctuality = c.Int(),
                        Quality = c.Int(),
                        ChanceToBeHiredAgain = c.Int(),
                        WantToCoworkAgain = c.Int(),
                        ManageSkills = c.Int(),
                        Project_ProjectId = c.Int(),
                        UserWhoAddRate_UserId = c.Int(),
                        UserWhoGetRate_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.RateId)
                .ForeignKey("dbo.ProjectModels", t => t.Project_ProjectId)
                .ForeignKey("dbo.UserBasicModels", t => t.UserWhoAddRate_UserId)
                .ForeignKey("dbo.UserBasicModels", t => t.UserWhoGetRate_UserId)
                .Index(t => t.Project_ProjectId)
                .Index(t => t.UserWhoAddRate_UserId)
                .Index(t => t.UserWhoGetRate_UserId);
            
            AddColumn("dbo.TaskModels", "TaskFinishDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PortfolioProjectModels", "ProjectName", c => c.String(nullable: false));
            AlterColumn("dbo.PortfolioProjectModels", "ProjectDescription", c => c.String(nullable: false));
            AlterColumn("dbo.OfferToProjectModels", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.TaskModels", "TaskName", c => c.String(nullable: false));
            AlterColumn("dbo.TaskModels", "TaskDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RateModels", "UserWhoGetRate_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.RateModels", "UserWhoAddRate_UserId", "dbo.UserBasicModels");
            DropForeignKey("dbo.RateModels", "Project_ProjectId", "dbo.ProjectModels");
            DropIndex("dbo.RateModels", new[] { "UserWhoGetRate_UserId" });
            DropIndex("dbo.RateModels", new[] { "UserWhoAddRate_UserId" });
            DropIndex("dbo.RateModels", new[] { "Project_ProjectId" });
            AlterColumn("dbo.TaskModels", "TaskDescription", c => c.String());
            AlterColumn("dbo.TaskModels", "TaskName", c => c.String());
            AlterColumn("dbo.OfferToProjectModels", "Description", c => c.String());
            AlterColumn("dbo.PortfolioProjectModels", "ProjectDescription", c => c.String());
            AlterColumn("dbo.PortfolioProjectModels", "ProjectName", c => c.String());
            DropColumn("dbo.TaskModels", "TaskFinishDate");
            DropTable("dbo.RateModels");
        }
    }
}
