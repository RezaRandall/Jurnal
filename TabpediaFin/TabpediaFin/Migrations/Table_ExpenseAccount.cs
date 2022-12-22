using FluentMigrator;

namespace TabpediaFin.Migrations;
[Migration(202212221031)]
public class Table_ExpenseAccount : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ExpenseAccount")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("ExpenseAccountNumber").AsString(250).NotNullable()
            .WithColumn("ExpenseCategoryId").AsInt32().NotNullable()
            .WithColumn("TaxId").AsInt32().Nullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
