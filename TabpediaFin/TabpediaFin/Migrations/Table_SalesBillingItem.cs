using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212081402)]
public class Table_SalesBillingItem : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("SalesBillingItem")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("TransId").AsInt32().NotNullable()
            .WithColumn("TaxId").AsInt32().NotNullable()
            .WithColumn("ItemId").AsInt32().NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("Price").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("TotalPrice").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("ItemUnitMeasureId").AsInt32().NotNullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
