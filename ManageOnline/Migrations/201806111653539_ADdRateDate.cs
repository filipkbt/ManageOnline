namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADdRateDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RateModels", "RateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RateModels", "RateDate");
        }
    }
}
