using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211210548)]
public class Table_Cash : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("Cash")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("GroupId").AsInt32().NotNullable()
            .WithColumn("BankName").AsString(250).NotNullable()
            .WithColumn("BankAccountNum").AsString(250).NotNullable()
            .WithColumn("BankAccountName").AsString(250).NotNullable()
            .WithColumn("CurrencyCode").AsString(250).NotNullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

    }
}
