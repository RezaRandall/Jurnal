using FluentMigrator;
using TabpediaFin.Domain;

namespace TabpediaFin.Migrations;

[Migration(202301121638)]
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
            .WithColumn("RecipientContactId").AsInt32().Nullable()
            .WithColumn("TransactionDate").AsDateTime().NotNullable()
            .WithColumn("TransactionNo").AsString(100).NotNullable()
            .WithColumn("Memo").AsString(250).Nullable()
            .WithColumn("TotalAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("DiscountAmount").AsDecimal(18, 2).Nullable().WithDefaultValue(0)
            .WithColumn("DiscountPercent").AsInt32().Nullable()
            .WithColumn("WitholdingAmount").AsDecimal(18, 2).Nullable().WithDefaultValue(0)
            .WithColumn("DiscountForAccountId").AsInt32().Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
