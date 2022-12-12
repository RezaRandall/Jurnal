using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212121618)]
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
            .WithColumn("TransNum").AsString(250).NotNullable()
            .WithColumn("TransDate").AsDateTime().NotNullable()
            .WithColumn("ContactId").AsInt32().NotNullable()
            .WithColumn("PaymentMethodId").AsInt32().NotNullable()
            .WithColumn("PaymentTermId").AsInt32().NotNullable()
            .WithColumn("Amount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("DiscountTypeId").AsInt32().NotNullable().WithDefaultValue(0) // 0-NoDiscount;1-Percent;2-Amount
            .WithColumn("DiscountPercent").AsDecimal(5, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("DiscountAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Notes").AsString(250).Nullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("TaxId").AsInt32().Nullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

    }
}
