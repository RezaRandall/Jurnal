using FluentMigrator;

namespace TabpediaFin.Migrations
{
    [Migration(202211141605)]
    public class Table_Customer : Migration
    {
        public override void Down()
        {
            Delete.Table("Master_Customer");
        }

        public override void Up()
        {
            Create.Table("Master_Customer")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("CustName").AsString(250).NotNullable()
                .WithColumn("Address").AsString(250).NotNullable()
                .WithColumn("Email").AsString(250).NotNullable()
                .WithColumn("Phone").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("CreatedUid").AsInt32().NotNullable()
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUid").AsInt32().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();
        }
    }
}
