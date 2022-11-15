using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211132014)]
public class Table_PaymentTerm : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("PaymentTerm")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("TermDays").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("PaymentTerm")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Net 30",
                    Description = "Net 30",
                    TermDays = 30,
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
        );

        Insert.IntoTable("PaymentTerm")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "COD",
                    Description = "Cash on Delivery",
                    TermDays = 0,
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
        );

        Insert.IntoTable("PaymentTerm")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Net 15",
                    Description = "Net 15",
                    TermDays = 15,
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
        );

        Insert.IntoTable("PaymentTerm")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Net 60",
                    Description = "Net 60",
                    TermDays = 60,
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
