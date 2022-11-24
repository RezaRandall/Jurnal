using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211241513)]
public class Table_Contact : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("Contact")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("CityName").AsString(250).Nullable()
            .WithColumn("PostalCode").AsString().Nullable()
            .WithColumn("Email").AsString(250).Nullable()
            .WithColumn("Phone").AsString(250).Nullable()
            .WithColumn("Fax").AsString(250).Nullable()
            .WithColumn("Website").AsString(250).Nullable()
            .WithColumn("Npwp").AsString(250).Nullable()
            .WithColumn("GroupId").AsInt32().NotNullable()
            .WithColumn("IsCustomer").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("IsVendor").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("IsEmployee").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("IsOther").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("Notes").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
