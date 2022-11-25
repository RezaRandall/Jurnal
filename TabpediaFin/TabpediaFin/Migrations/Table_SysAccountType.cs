using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211241919)]
public class Table_SysAccountType : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("SysAccountType")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("Name").AsString(250).NotNullable()
            .WithColumn("Description").AsString(250).Nullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("SysAccountType")
            .Row(
                new
                {
                    Id = 1,
                    Name = "Asset",
                    Description = "Asset",
                    CreatedUid = 0,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("SysAccountType")
            .Row(
                new
                {
                    Id = 2,
                    Name = "Liability",
                    Description = "Liability",
                    CreatedUid = 0,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("SysAccountType")
            .Row(
                new
                {
                    Id = 3,
                    Name = "Equity",
                    Description = "Equity",
                    CreatedUid = 0,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("SysAccountType")
            .Row(
                new
                {
                    Id = 4,
                    Name = "Revenue",
                    Description = "Revenue",
                    CreatedUid = 0,
                    CreatedUtc = DateTime.UtcNow
                }
            );

        Insert.IntoTable("SysAccountType")
            .Row(
                new
                {
                    Id = 5,
                    Name = "Expense",
                    Description = "Expense",
                    CreatedUid = 0,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
