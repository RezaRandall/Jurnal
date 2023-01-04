using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212161115)]
public class Table_AccountCategory : Migration
{
    public override void Down()
    {

    }

    //kategori akun tidak perlu tenant id, untuk umum
    public override void Up()
    {
        Create.Table("AccountCategory")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("Name").AsString(50).Nullable()
            .WithColumn("StartAccountCode").AsString(50).Nullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Kas & Bank",
                    StartAccountCode = "1-10001",
                    CreatedUtc = DateTime.UtcNow
                }
        );

        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Akun Piutang",
                    StartAccountCode = "1-10100",
                    CreatedUtc = DateTime.UtcNow
                }
        );

        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Persediaan",
                    StartAccountCode = "1-10200",
                    CreatedUtc = DateTime.UtcNow
                }
        );

        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Aktiva Lancar Lainnya",
                    StartAccountCode = "1-10300",
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Aktiva Tetap",
                    StartAccountCode = "1-10700",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Depresiasi & Amortisasi",
                    StartAccountCode = "1-10751",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Aktiva Lainnya",
                    StartAccountCode = "1-10800",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Akun Hutang",
                    StartAccountCode = "2-20100",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Kewajiban Lancar Lainnya",
                    StartAccountCode = "2-20200",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Ekuitas",
                    StartAccountCode = "3-30000",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Pendapatan",
                    StartAccountCode = "4-40000",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Harga Pokok Penjualan",
                    StartAccountCode = "5-50000",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Beban",
                    StartAccountCode = "6-60000",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Pendapatan Lainnya",
                    StartAccountCode = "7-70000",
                    CreatedUtc = DateTime.UtcNow
                }
            );
        Insert.IntoTable("AccountCategory")
            .Row(
                new
                {
                    Name = "Beban Lainnya",
                    StartAccountCode = "7-70000",
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
