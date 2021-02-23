using FluentMigrator;

namespace WebdevPeriod3.Migrations
{
    [Migration(20210204131100)]
    public class AddBusinessLogic : Migration
    {
        public override void Up()
        {
            // Create a table called "Products"
            Create.Table("Products")
                .WithColumn("Id").AsString().PrimaryKey()
                .WithColumn("Name").AsString()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("PosterId").AsString().Nullable().ForeignKey("Users", "Id").OnDelete(System.Data.Rule.SetNull)
                .WithColumn("ShowInCatalog").AsBoolean().WithDefaultValue(false)
                .WithColumn("CreatedAt").AsDateTime()
                .WithColumn("Comments").AsInt32();

            // Create a table called "ProductRelations"
            Create.Table("ProductRelations")
                .WithColumn("ProductId").AsString().ForeignKey("Products", "Id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("SubProductId").AsString().ForeignKey("Products", "Id").OnDelete(System.Data.Rule.Cascade);

            Create.PrimaryKey("PK_ProductRelations").OnTable("ProductRelations").Columns("ProductId", "SubProductId");

            // Create a table called "Comments"
            Create.Table("Comments")
                .WithColumn("Id").AsString().PrimaryKey()
                .WithColumn("ProductId").AsString().ForeignKey("Products", "Id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("ParentId").AsString().Nullable().ForeignKey("Comments", "Id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("PosterId").AsString().Nullable().ForeignKey("Users", "Id").OnDelete(System.Data.Rule.SetNull)
                .WithColumn("Content").AsString().Nullable()
                .WithColumn("Flagged").AsBoolean().WithDefaultValue(false);
        }

        public override void Down()
        {
            // Drop Comments, ProductRelations and Products
            Delete.Table("Comments");
            Delete.Table("ProductRelations");
            Delete.Table("Products");
        }
    }
}
