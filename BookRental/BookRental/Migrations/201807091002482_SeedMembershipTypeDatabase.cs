namespace BookRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedMembershipTypeDatabase : DbMigration
    {
        public override void Up()
        {
            Sql("insert into [dbo].[MembershipTypes](Name, SignUpFee, ChargeRateOneMonth, ChargeRateSixMonth) values('Pay per Rental', 0, 50, 25)");
            Sql("insert into [dbo].[MembershipTypes](Name, SignUpFee, ChargeRateOneMonth, ChargeRateSixMonth) values('Member', 150, 20, 10)");
            Sql("insert into [dbo].[MembershipTypes](Name, SignUpFee, ChargeRateOneMonth, ChargeRateSixMonth) values('SuperAdmin', 0, 0, 0)");
        }

        public override void Down()
        {
        }
    }
}
