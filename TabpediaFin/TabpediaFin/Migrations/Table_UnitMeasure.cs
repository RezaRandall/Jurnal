using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211131043)]
public class Table_UnitMeasure : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("UnitMeasure")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("UnitMeasure")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "buah",
                    Description = "buah",
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
        );
    }
}
