using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211131656)]
public class Table_PaymentMethod : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("PaymentMethod")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("PaymentMethod")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Kas Tunai",
                    Description = "Kas Tunai",
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("PaymentMethod")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Cek dan Giro",
                    Description = "Cek dan Giro",
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("PaymentMethod")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Transfer Bank",
                    Description = "Transfer Bank",
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("PaymentMethod")
            .Row(
                new
                {
                    TenantId = 1,
                    Name = "Kartu Kredit",
                    Description = "Kartu Kredit",
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}


//[Migration(202211132047)]
//public class Table_PaymentMethod_202211132047 : Migration
//{
//    public override void Down()
//    {

//    }

//    public override void Up()
//    {
//        Create.Index("UQ_Name").OnTable("PaymentMethod").OnColumn
//    }
//}
