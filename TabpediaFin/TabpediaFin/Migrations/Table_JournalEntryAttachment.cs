using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202301011017)]
public class Table_JournalEntryAttachment : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("JournalEntryAttachment")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("FileName").AsString(255).NotNullable()
            .WithColumn("FileUrl").AsString(255).NotNullable()
            .WithColumn("Extension").AsString(255).NotNullable()
            .WithColumn("FileSize").AsString(255).NotNullable()
            .WithColumn("TransId").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
