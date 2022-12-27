using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212271112)]
public class Table_ReceiveMoneyList : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ReceiveMoneyList")
        .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
        .WithColumn("TenantId").AsInt32().NotNullable()
        .WithColumn("ReceiveMoneyId").AsInt32().NotNullable()
        .WithColumn("PriceIncludesTax").AsBoolean().NotNullable().WithDefaultValue(false)
        .WithColumn("ReceiveFromAccountId").AsInt32().NotNullable()
        .WithColumn("Description").AsString(100).Nullable()
        .WithColumn("TaxId").AsInt32().Nullable()
        .WithColumn("Amount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0);
    }
}
