using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211241538)]
public class Table_ContactPerson : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ContactPerson")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("ContactId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Email").AsString(250).Nullable()
            .WithColumn("Phone").AsString(250).Nullable()
            .WithColumn("Fax").AsString(250).Nullable()
            .WithColumn("Others").AsString(250).Nullable()
            .WithColumn("Notes").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
