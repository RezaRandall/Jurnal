using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211051330)]
public class Table_AppRole : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("AppRole")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("AppId").AsInt32().NotNullable()
            .WithColumn("RoleName").AsString(250).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("AppRole")
            .Row(
                new
                {
                    AppId = 100,
                    RoleName = "Administrator",
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("AppRole")
            .Row(
                new
                {
                    AppId = 100,
                    RoleName = "Staff",
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
