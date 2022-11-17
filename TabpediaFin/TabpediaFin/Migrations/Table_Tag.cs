using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211171107)]
public class Table_Tag : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("Tag")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("Tag")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Project A",
                    Description = "Project A",
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
        );
    }
}