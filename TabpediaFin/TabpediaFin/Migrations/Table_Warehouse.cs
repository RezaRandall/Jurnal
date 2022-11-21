using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211210601)]
public class Table_Warehouse : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("Warehouse")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("Address").AsString(250).NotNullable()
            .WithColumn("CityName").AsString(250).NotNullable()
            .WithColumn("PostalCode").AsString(250).NotNullable()
            .WithColumn("Phone").AsString(250).NotNullable()
            .WithColumn("Fax").AsString(250).NotNullable()
            .WithColumn("Email").AsString(250).NotNullable()
            .WithColumn("ContactPersonName").AsString(250).NotNullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

    }
}
