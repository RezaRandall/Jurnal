using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211250735)]
public class Table_ExpenseCategory : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ExpenseCategory")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("AccountId").AsInt32().NotNullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("ExpenseCategory")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Transport",
                    Description = "Biaya Transport",
                    AccountId = 0,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}