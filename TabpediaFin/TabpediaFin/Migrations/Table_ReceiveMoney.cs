using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202301021438)]
public class Table_ReceiveMoney : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ReceiveMoney")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("DepositToAccountId").AsInt32().NotNullable()
            .WithColumn("PayerId").AsInt32().Nullable()
            .WithColumn("TransactionDate").AsDateTime().NotNullable()
            .WithColumn("TransactionNo").AsString(100).NotNullable()
            .WithColumn("Memo").AsString(250).Nullable()
            .WithColumn("TotalAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
