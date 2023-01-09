using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212051560)]
public class Table_Account : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("Account")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("AccountNumber").AsString(250).NotNullable()
            .WithColumn("CategoryId").AsInt32().Nullable().WithDefaultValue(0)
            //.WithColumn("DetailAccount").AsInt32().NotNullable().WithDefaultValue(0) //1 - Header dari, 2 - Sub Akun dari
            .WithColumn("AccountParentId").AsInt32().Nullable().WithDefaultValue(0)
            .WithColumn("BankId").AsInt32().Nullable().WithDefaultValue(0)
            .WithColumn("TaxId").AsInt32().Nullable().WithDefaultValue(0)
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("Balance").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("IsLocked").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        //Alter.Table("AccountCashAndBank")
        //    .AddColumn("Balance").AsDecimal(18, 2).NotNullable().WithDefaultValue(0);
    }
}