using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211210505)]
public class Table_ItemItemCatgeory : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ItemItemCategory")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("ItemId").AsInt32().NotNullable()
            .WithColumn("ItemCategoryId").AsInt32().NotNullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

    }
}
