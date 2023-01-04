using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212161116)]
public class Table_BankMaster : Migration
{
    public override void Down()
    {

    }

    //kategori akun tidak perlu tenant id, untuk umum
    public override void Up()
    {
        Create.Table("MasterBank")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("Name").AsString(50).Nullable()
            .WithColumn("BankCode").AsString(50).Nullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
