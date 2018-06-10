namespace ManageOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeFloatToDoubleAtRate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RateModels", "AverageRate", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RateModels", "AverageRate", c => c.Single(nullable: false));
        }
    }
}
