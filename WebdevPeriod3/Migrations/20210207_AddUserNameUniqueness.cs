using FluentMigrator;

namespace WebdevPeriod3.Migrations
{
    [Migration(20210207171300)]
    public class AddUserNameUniqueness : Migration
    {
        public override void Up()
        {
            Create.UniqueConstraint().OnTable("Users").Column("UserName");
            Create.UniqueConstraint().OnTable("Users").Column("NormalizedUserName");
        }

        public override void Down()
        {
            Delete.UniqueConstraint().FromTable("Users").Column("UserName");
            Delete.UniqueConstraint().FromTable("Users").Column("NormalizedUserName");
        }
    }
}
