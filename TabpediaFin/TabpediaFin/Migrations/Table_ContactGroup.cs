using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211241502)]
public class Table_ContactGroup : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ContactGroup")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("ContactGroup")
            .Row(
                new
                {
                    Name = "Perorangan",
                    TenantId = 1,
                    Description = "Perorangan",
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            ).Row(
                new
                {
                    Name = "Perusahaan",
                    TenantId = 1,
                    Description = "Perusahaan",
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
