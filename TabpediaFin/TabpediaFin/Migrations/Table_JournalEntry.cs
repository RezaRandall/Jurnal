using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202301011018)]
public class Table_JournalEntry : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("JournalEntry")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("TransDate").AsDateTime().NotNullable()
            .WithColumn("TransCode").AsString(250).NotNullable()
            .WithColumn("Memo").AsString(250).Nullable()
            .WithColumn("Notes").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
