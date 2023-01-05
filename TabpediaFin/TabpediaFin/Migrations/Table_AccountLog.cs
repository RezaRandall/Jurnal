using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212051559)]
public class Table_AccountLog : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("AccountLog")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("AccountId").AsInt32().NotNullable()
            .WithColumn("ContactId").AsInt32().NotNullable()
            .WithColumn("Description").AsString(250).NotNullable()
            .WithColumn("AccountNumber").AsString(250).NotNullable()
            .WithColumn("AccountCategoryId").AsInt32().Nullable()            
            .WithColumn("Credit").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Debit").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        //Alter.Table("AccountCashAndBank")
        //    .AddColumn("Balance").AsDecimal(18, 2).NotNullable().WithDefaultValue(0);
    }
}