using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211241511)]
public class Table_ContactAddressType : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ContactAddressType")
           .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
           .WithColumn("TenantId").AsInt32().NotNullable()
           .WithColumn("Name").AsString(250).NotNullable()
           .WithColumn("Description").AsString(250).Nullable()
           .WithColumn("CreatedUid").AsInt32().NotNullable()
           .WithColumn("CreatedUtc").AsDateTime().NotNullable()
           .WithColumn("UpdatedUid").AsInt32().Nullable()
           .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("ContactAddressType")
            .Row(
                new
                {
                    Name = "Alamat Pengiriman",
                    TenantId = 1,
                    Description = "Alamat Pengiriman",
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            ).Row(
                new
                {
                    Name = "Alamat Penagihan",
                    TenantId = 1,
                    Description = "Alamat Penagihan",
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
