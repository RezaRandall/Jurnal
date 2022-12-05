using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212021335)]
public class Table_PurchaseRequest : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("PurchaseRequest")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("StaffId").AsInt32().NotNullable()
            .WithColumn("VendorId").AsInt32().NotNullable()
            .WithColumn("TransDate").AsDateTime().NotNullable()
            .WithColumn("DueDate").AsDateTime().NotNullable()
            .WithColumn("TransCode").AsString(250).NotNullable()
            .WithColumn("BudgetYear").AsString(4).NotNullable()
            .WithColumn("UrgentLevel").AsInt32().NotNullable().WithDefaultValue(0) // 0-low;1-moderate;2-high
            .WithColumn("Memo").AsString(250).Nullable()
            .WithColumn("Notes").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
