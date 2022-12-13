using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211210429)]
public class Table_ItemStock : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ItemStock")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("WarehouseId").AsInt32().NotNullable()
            .WithColumn("ItemId").AsInt32().NotNullable()
            .WithColumn("Quantity").AsDecimal(18,0).NotNullable().WithDefaultValue(0)
            //.WithColumn("Type").AsInt32().NotNullable()//0 : pengurangan, 1: penjumlahan
            .WithColumn("Description").AsString(250).NotNullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
