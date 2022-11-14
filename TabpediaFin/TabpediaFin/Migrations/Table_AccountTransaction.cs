using FluentMigrator;

namespace TabpediaFin.Migrations
{
    [Migration(202211141500)]
    public class Table_AccountTransaction : Migration
    {
        public override void Down()
        {
            Delete.Table("AccountTransaction");
        }

        public override void Up()
        {
            Create.Table("AccountTransaction")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("accountId").AsInt32().NotNullable()
                .WithColumn("amount").AsDouble().NotNullable()
                .WithColumn("type").AsInt32().NotNullable().WithDefaultValue("0").WithColumnDescription("0:IN, 1:Out")
                .WithColumn("description").AsString(255).NotNullable().WithDefaultValue("No description")
                .WithColumn("CreatedUid").AsInt32().NotNullable()
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUid").AsInt32().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();
        }
    }
}
