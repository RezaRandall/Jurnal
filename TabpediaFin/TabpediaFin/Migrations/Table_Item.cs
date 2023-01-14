using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211210428)]
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
            .WithColumn("Code").AsString(250).NotNullable()
            .WithColumn("Barcode").AsString(250).Nullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("UnitMeasureId").AsInt32().NotNullable()
            .WithColumn("AverageCost").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Cost").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Price").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("IsSales").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("IsPurchase").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("IsStock").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("StockMin").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("IsArchived").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("ImageFileName").AsString(250).Nullable()
            .WithColumn("Notes").AsString(250).Nullable()
            .WithColumn("PurchaseAccount").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("SalesAccount").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

    }
}
