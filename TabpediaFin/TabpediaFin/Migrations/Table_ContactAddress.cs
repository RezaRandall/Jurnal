using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211241535)]
public class Table_ContactAddress : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ContactAddress")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("ContactId").AsInt32().NotNullable()
            .WithColumn("AddressName").AsString(250).Nullable()
            .WithColumn("Address").AsString(250).NotNullable()
            .WithColumn("CityName").AsString(250).Nullable()
            .WithColumn("PostalCode").AsString().Nullable()
            .WithColumn("AddressTypeId").AsInt32().NotNullable().WithDefaultValue(1)
            .WithColumn("Notes").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
