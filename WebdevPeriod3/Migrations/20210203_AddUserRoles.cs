using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdevPeriod3.Migrations
{
    [Migration(20210203170800)]
    public class AddUserRoles : Migration
    {
        public override void Up()
        {
            Create.Table("UserRoles")
                .WithColumn("RoleId").AsString().ForeignKey("Roles", "Id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("UserId").AsString().ForeignKey("Users", "Id").OnDelete(System.Data.Rule.Cascade);

            Create.PrimaryKey("PK_UserRoles").OnTable("UserRoles").Columns("RoleId", "UserId");
        }

        public override void Down()
        {
            Delete.Table("UserRoles");
        }
    }
}
