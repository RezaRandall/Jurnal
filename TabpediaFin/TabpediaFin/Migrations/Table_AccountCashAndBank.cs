using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212051531)]
public class Table_Account : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("AccountCashAndBank")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("AccountNumber").AsString(250).NotNullable()
            .WithColumn("CashAndBankCategoryId").AsInt32().Nullable()
            .WithColumn("DetailAccountId").AsInt32().Nullable()
            .WithColumn("TaxId").AsInt32().Nullable()
            .WithColumn("BankId").AsInt32().Nullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}