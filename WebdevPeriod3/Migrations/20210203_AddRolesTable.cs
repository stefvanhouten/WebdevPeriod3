using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdevPeriod3.Migrations
{
    [Migration(20210203160100)]
    public class AddRolesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Roles")
                .WithColumn("ConcurrencyStamp").AsString().Nullable()
                .WithColumn("Id").AsString().PrimaryKey()
                .WithColumn("Name").AsString().Unique()
                .WithColumn("NormalizedName").AsString().Unique();
        }

        public override void Down()
        {
            Delete.Table("Roles");
        }
    }
}
