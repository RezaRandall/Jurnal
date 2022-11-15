using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211141209)]
public class Table_Item : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("Item")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("CategoryId").AsInt32().NotNullable()
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
