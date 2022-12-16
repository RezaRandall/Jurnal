using FluentMigrator;
using TabpediaFin.Domain;

namespace TabpediaFin.Migrations;

[Migration(202212151048)]
public class Table_SendMoney : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("SendMoney")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("PayFromAccountId").AsInt32().NotNullable()
            .WithColumn("ReceiverVendorId").AsInt32().Nullable()
            .WithColumn("TransactionDate").AsDateTime().NotNullable()
            .WithColumn("TransactionNo").AsString(100).NotNullable()
            .WithColumn("PriceIncludesTax").AsBoolean().NotNullable().WithDefaultValue(false) 
            .WithColumn("AccountCashAndBankId").AsInt32().Nullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("TaxId").AsInt32().Nullable()
            .WithColumn("Amount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Memo").AsString(250).Nullable()
            .WithColumn("TotalAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("DiscountAmount").AsDecimal(18, 2).Nullable().WithDefaultValue(0)
            .WithColumn("DiscountPercent").AsInt32().Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
