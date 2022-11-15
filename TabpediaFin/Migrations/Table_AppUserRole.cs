using FluentMigrator;

namespace Tabpedia.Migrations;

[Migration(202211061442)]
public class Table_AppUserRole : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("AppUserRole")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("UserId").AsInt32().NotNullable()
            .WithColumn("RoleId").AsInt32().NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("AppUserRole")
            .Row(
                new
                {
                    UserId = 1,
                    RoleId = 1,
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
