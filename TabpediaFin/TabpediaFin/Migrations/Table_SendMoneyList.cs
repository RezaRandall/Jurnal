using FluentMigrator;

namespace TabpediaFin.Migrations;

    [Migration(202301021633)]
public class Table_SendMoneyList : Migration
{
    public override void Down()
    {
        
    }

    public override void Up()
    {
        Create.Table("SendMoneyList")
        .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
        .WithColumn("TenantId").AsInt32().NotNullable()
        .WithColumn("PriceIncludesTax").AsBoolean().NotNullable().WithDefaultValue(false)
        .WithColumn("PaymentForAccountCashAndBanktId").AsInt32().NotNullable()
        .WithColumn("Description").AsString(250).Nullable()
        .WithColumn("TaxId").AsInt32().Nullable()
        .WithColumn("Amount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
        .WithColumn("TransId").AsInt32().NotNullable().WithDefaultValue(0)
        .WithColumn("CreatedUid").AsInt32().NotNullable()
        .WithColumn("CreatedUtc").AsDateTime().NotNullable()
        .WithColumn("UpdatedUid").AsInt32().Nullable()
        .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
