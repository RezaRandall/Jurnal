using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211051333)]
public class Table_AppTenant : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("AppTenant")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("Code").AsString().NotNullable().Unique()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Address").AsString(250).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("IsLocked").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("AppTenant")
            .Row(
                new
                {
                    Code = Guid.NewGuid().ToString("N").ToUpper(),
                    Name = "Techpark Computer",
                    Address = "Mall Central Park Jakarta",
                    IsActive = true,
                    IsLocked = false,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
