using FluentMigrator;

namespace TabpediaFin.Migrations
{
    [Migration(202211141304)]
    public class Table_CoA : Migration
    {
        public override void Down()
        {
            Delete.Table("Master_Account");
        }

        public override void Up()
        {
            Create.Table("Master_Account")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("AccountName").AsString(250).NotNullable()
                .WithColumn("Category").AsString(250).NotNullable().WithDefaultValue("Cash & Bank")
                .WithColumn("Tax").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("CreatedUid").AsInt32().NotNullable()
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUid").AsInt32().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();
        }
    }
}
