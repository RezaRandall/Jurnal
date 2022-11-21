using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211210531)]
public class Table_CashGroup : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("CashGroup")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("CashGroup")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Tunai",
                    Description = "Tunai",
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("CashGroup")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Bank",
                    Description = "Bank",
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("CashGroup")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "E-Money",
                    Description = "E-Money",
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}