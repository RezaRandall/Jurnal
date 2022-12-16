using FluentMigrator;

namespace TabpediaFin.Migrations;
[Migration(202212161114)]
public class Table_BankName : Migration
{
    public override void Down()
    {}

    public override void Up()
    {
     Create.Table("BankName")
        .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
        .WithColumn("TenantId").AsInt32().NotNullable()
        .WithColumn("Name").AsString(50).Nullable()
        .WithColumn("CreatedUid").AsInt32().NotNullable()
        .WithColumn("CreatedUtc").AsDateTime().NotNullable()
        .WithColumn("UpdatedUid").AsInt32().Nullable()
        .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
