using FluentMigrator;

namespace WebdevPeriod3.Migrations
{
    [Migration(202103211457)]
    public class AddProductImage : Migration
    {
        public override void Up()
        {
            Create.Column("Image").OnTable("Products").AsBinary();
        }

        public override void Down()
        {
            Delete.Column("Image").FromTable("Products");
        }
    }
}
