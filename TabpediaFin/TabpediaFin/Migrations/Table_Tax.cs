using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211210356)]
public class Table_Tax : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("Tax")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("RatePercent").AsDecimal(5, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("Tax")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "PPN",
                    Description = "PPN",
                    RatePercent = 11,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
        );
    }
}