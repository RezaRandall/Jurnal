using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212081404)]
public class Table_SalesOrder : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("SalesOrder")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("StaffId").AsInt32().NotNullable()
            .WithColumn("VendorId").AsInt32().NotNullable()
            .WithColumn("WarehouseId").AsInt32().NotNullable()
            .WithColumn("TransDate").AsDateTime().NotNullable()
            .WithColumn("DueDate").AsDateTime().NotNullable()
            .WithColumn("TransCode").AsString(250).NotNullable()
            .WithColumn("Status").AsInt32().NotNullable().WithDefaultValue(0) // 0-Open;1-Closed;jika status open dan due date terlewati maka akan menjadi expired
            .WithColumn("DiscountType").AsInt32().NotNullable().WithDefaultValue(0) //0-presentase; 1-nominal
            .WithColumn("DiscountAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Memo").AsString(250).Nullable()
            .WithColumn("Notes").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
