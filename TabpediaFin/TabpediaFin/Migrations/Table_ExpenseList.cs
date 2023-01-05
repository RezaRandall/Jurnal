using FluentMigrator;

namespace TabpediaFin.Migrations;
[Migration(202301051655)]
public class Table_ExpenseList : Migration
{
    public override void Down()
    {
        
    }

    public override void Up()
    {
        Create.Table("ExpenseList")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("PriceIncludesTax").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("ExpenseAccountId").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("TaxId").AsInt32().Nullable().WithDefaultValue(0)
            .WithColumn("Amount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("TransId").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}

