namespace IoTPoolAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PoolName = c.String(),
                        PoolWaterTempC = c.Double(nullable: false),
                        OutsideAirTempC = c.Double(nullable: false),
                        IsPoolPowerOn = c.Int(nullable: false),
                        SampleDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pools");
        }
    }
}
