using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202211061755)]
public class Table_AppUser : Migration
{
    private readonly IPasswordHasher _passwordHasher;

    public Table_AppUser(IPasswordHasher passwordHasher)
        : base()
    {
        _passwordHasher = passwordHasher;
    }

    public override void Down()
    {
        
    }

    public override void Up()
    {
        Create.Table("AppUser")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("Username").AsString(250).NotNullable().Unique()
            .WithColumn("Hashed").AsString(250).Nullable()
            .WithColumn("Salt").AsString(250).Nullable()
            .WithColumn("FullName").AsString(250).Nullable()
            .WithColumn("TenantId").AsInt32().Nullable()
            .WithColumn("Email").AsString(250).NotNullable()
            .WithColumn("EmailConfirmed").AsBoolean().WithDefaultValue(false)
            .WithColumn("PhoneNumber").AsString(250).Nullable()
            .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("IsLocked").AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

        var salt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        Insert.IntoTable("AppUser")
            .Row(
                new
                {
                    Username = "demo@techpark.co",
                    Hashed = _passwordHasher.Hash("password123", salt),
                    Salt = salt,
                    FullName = "John Doe",
                    TenantId = 1,
                    Email = "test@demo.com",
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedUid = 1,
                    CreatedUtc = DateTime.UtcNow
                }
            );
    }
}
