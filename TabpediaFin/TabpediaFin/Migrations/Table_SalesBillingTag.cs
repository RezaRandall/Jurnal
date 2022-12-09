using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212081401)]
public class Table_SalesBillingTag : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("SalesBillingTag")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("TagId").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("TransId").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}
