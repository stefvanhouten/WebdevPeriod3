using FluentMigrator;

namespace WebdevPeriod3.Migrations
{
    [Migration(20210209172300)]
    public class AddEmailUniqueness : Migration
    {
        public override void Up()
        {
            Create.UniqueConstraint().OnTable("Users").Column("Email");
        }

        public override void Down()
        {
            Delete.UniqueConstraint().FromTable("Users").Column("Email");
        }
    }
}
