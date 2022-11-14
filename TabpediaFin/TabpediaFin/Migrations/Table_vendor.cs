using FluentMigrator;

namespace TabpediaFin.Migrations
{
    [Migration(202211141600)]
    public class Table_vendor:Migration
    {
        
        public override void Up()
        {
            Create.Table("Master_Vendor")
                .WithColumn("vendor_id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("badan_usaha").AsString(250).NotNullable()
                .WithColumn("npwp_badan").AsString(250).NotNullable()
                .WithColumn("rekening").AsString(250).NotNullable()
                .WithColumn("bank").AsString(250).NotNullable()
                .WithColumn("nama_direktur").AsString(250).NotNullable()
                .WithColumn("email").AsString(250).NotNullable()
                .WithColumn("telp").AsString(250).NotNullable()
                .WithColumn("alamat").AsString(250).NotNullable()
                .WithColumn("kop").AsString(250).NotNullable()
                .WithColumn("akta_notaris").AsString(250).NotNullable()
                .WithColumn("CreatedUid").AsInt32().NotNullable()
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUid").AsInt32().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("Master_Vendor");
        }
    }
}
