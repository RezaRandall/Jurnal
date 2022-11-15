using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211051301)]
public class Table_App : Migration
{
    public override void Down()
    {
        
    }

    public override void Up()
    {
        Create.Table("App")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("AppCode").AsString(250).NotNullable().Unique()
            .WithColumn("AppName").AsString(250).NotNullable()
            .WithColumn("AppDisplayName").AsString(250).NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        Insert.IntoTable("App")
            .Row(
                new
                {
                    Id = 100,
                    AppCode = Guid.NewGuid().ToString("N").ToUpper(),
                    AppName = "Tabpedia Finance",
                    AppDisplayName = "Tabpedia Finance",
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
