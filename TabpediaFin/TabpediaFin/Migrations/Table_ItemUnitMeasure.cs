using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211210501)]
public class Table_ItemUnitMeasure : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ItemUnitMeasure")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("ItemId").AsInt32().NotNullable()
            .WithColumn("UnitMeasureId").AsInt32().NotNullable()
            .WithColumn("UnitConversion").AsDecimal(18, 2).NotNullable().WithDefaultValue(decimal.One)
            .WithColumn("Cost").AsDecimal(18, 2).NotNullable().WithDefaultValue(decimal.Zero)
            .WithColumn("Price").AsDecimal(18, 2).NotNullable().WithDefaultValue(decimal.Zero)
            .WithColumn("Notes").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

    }
}
