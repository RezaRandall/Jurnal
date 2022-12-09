using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212071706)]
public class Table_AccountCashAndBankCategory : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("AccountCashAndBankCategory")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("Name").AsString(50).Nullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

    }
}
