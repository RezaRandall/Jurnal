using FluentMigrator;

namespace Tabpedia.Migrations;

[Migration(202211061820)]
public class Table_AppRefreshToken : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("AppRefreshToken")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("Token").AsString(250).NotNullable()
            .WithColumn("ExpiredUtc").AsString(250).NotNullable()
            .WithColumn("UserId").AsInt32().NotNullable().Unique()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
    }
}
