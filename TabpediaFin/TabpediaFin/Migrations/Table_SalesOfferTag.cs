using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212081409)]
public class Table_SalesOfferTag : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("SalesOfferTag")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("TagId").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("TransId").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
