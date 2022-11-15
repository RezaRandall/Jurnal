﻿using FluentMigrator;

namespace TabpediaFin.Migrations
{
    [Migration(202211141346)]
    public class Table_TransferFunds : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Create.Table("TransferFunds")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("TransferFrom").AsString(250).NotNullable()
                .WithColumn("DepositTo").AsString(250).NotNullable()
                .WithColumn("Amount").AsInt32().NotNullable()
                .WithColumn("Tags").AsString(250).Nullable()
                .WithColumn("TransactionNo").AsString(250).NotNullable()
                .WithColumn("Memo").AsString(250).NotNullable()
                .WithColumn("Attachment").AsString(250).Nullable()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("TenantId").AsInt32().NotNullable()

                .WithColumn("AccountCode").AsString(250).Nullable()
                .WithColumn("AccountName").AsString(250).Nullable()
                .WithColumn("StatementBalance").AsInt32().Nullable()
                .WithColumn("BalanceInJournal").AsInt32().Nullable()

                .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("IsLocked").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CreatedUid").AsInt32().NotNullable()
                .WithColumn("CreatedUtc").AsDateTime().NotNullable()
                .WithColumn("UpdatedUid").AsInt32().Nullable()
                .WithColumn("UpdatedUtc").AsDateTime().Nullable();


            var salt = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            Insert.IntoTable("TransferFunds")
                .Row(
                    new
                    {
                        TransferFrom = "Canada",
                        DepositTo = "New York",
                        Amount = 1000000,
                        Tags = "Paid",
                        TransactionNo = 123123123,
                        Memo = "inet payment",
                        AccountCode = "1-10001",
                        AccountName = "John Doe",
                        StatementBalance = 4000000,
                        BalanceInJournal = 4000000,
                        UserId = 1,
                        TenantId = 1,
                        IsActive = true,
                        CreatedUid = 1,
                        CreatedUtc = DateTime.UtcNow
                    }
                );
        }
    }
}
