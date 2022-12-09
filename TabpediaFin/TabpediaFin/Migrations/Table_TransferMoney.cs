using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212071555)]
public class Table_TransferMoney : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("TransferMoney")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TransferFromAccountId").AsInt32().NotNullable()
            .WithColumn("DepositToAccountId").AsInt32().NotNullable()
            .WithColumn("Amount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Memo").AsString(250).Nullable()
            .WithColumn("TransactionNumber").AsString(250).NotNullable()
            .WithColumn("TransactionDate").AsDateTime().NotNullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

    }
}
