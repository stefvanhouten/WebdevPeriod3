using FluentMigrator;

namespace WebdevPeriod3.Migrations
{
    [Migration(20210209172900)]
    public class AddNormalizedEmailUniqueness : Migration
    {
        public override void Up()
        {
            Create.UniqueConstraint().OnTable("Users").Column("NormalizedEmail");
        }

        public override void Down()
        {
            Delete.UniqueConstraint().FromTable("Users").Column("NormalizedEmail");
        }
    }
}
