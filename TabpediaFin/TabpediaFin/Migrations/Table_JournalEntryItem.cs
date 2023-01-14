using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202301011016)]
public class Table_JournalEntryItem : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("JournalEntryItem")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("TransId").AsInt32().NotNullable()
            .WithColumn("AccountId").AsInt32().NotNullable()
            .WithColumn("ItemId").AsInt32().NotNullable()
            .WithColumn("Description").AsString(255).Nullable()
            .WithColumn("Debit").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Credit").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
