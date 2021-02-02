using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdevPeriod3.Migrations
{
    [Migration(20210202151100)]
    public class AddUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("AccessFailedCount").AsInt32().WithDefaultValue(0)
                .WithColumn("ConcurrencyStamp").AsString().NotNullable()
                .WithColumn("Email").AsString()
                .WithColumn("EmailConfirmed").AsBoolean().WithDefaultValue(false)
                .WithColumn("Id").AsString().PrimaryKey()
                .WithColumn("LockoutEnabled").AsBoolean().WithDefaultValue(false)
                .WithColumn("LockoutEnd").AsDateTime().Nullable()
                .WithColumn("NormalizedEmail").AsString()
                .WithColumn("NormalizedUserName").AsString()
                .WithColumn("PasswordHash").AsString().Nullable()
                .WithColumn("PhoneNumber").AsString().Nullable()
                .WithColumn("PhoneNumberConfirmed").AsBoolean().WithDefaultValue(false)
                .WithColumn("SecurityStamp").AsString()
                .WithColumn("TwoFactorEnabled").AsBoolean().WithDefaultValue(false)
                .WithColumn("UserName").AsString();
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}
