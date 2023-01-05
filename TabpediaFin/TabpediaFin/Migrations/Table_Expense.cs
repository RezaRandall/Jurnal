using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202301051653)]
public class Table_Expense : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("Expense")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("PayFromAccountId").AsInt32().NotNullable()
            .WithColumn("PayLater").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("RecipientContactId").AsInt32().Nullable()
            .WithColumn("TransactionDate").AsDateTime().NotNullable()
            .WithColumn("PaymentMethodId").AsInt32().NotNullable()
            .WithColumn("TransactionNo").AsString(100).NotNullable()
            .WithColumn("BillingAddress").AsString(250).Nullable()
            .WithColumn("DueDate").AsDateTime().Nullable()
            .WithColumn("PaymentTermId").AsInt32().Nullable()
            .WithColumn("Memo").AsString(250).Nullable()
            .WithColumn("Status").AsInt32().NotNullable().WithDefaultValue(0) // 0-Open;1-Closed;jika status open dan due date terlewati maka akan menjadi expired
            .WithColumn("DiscountPercent").AsInt32().NotNullable().WithDefaultValue(0) //0-presentase; 1-nominal
            .WithColumn("DiscountAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("TotalAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

    }
}
