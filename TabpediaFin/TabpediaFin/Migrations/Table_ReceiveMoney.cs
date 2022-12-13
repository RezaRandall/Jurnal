﻿using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212071605)]
public class Table_ReceiveMoney : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ReceiveMoney")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("DepositToAccountId").AsInt32().NotNullable()
            .WithColumn("VendorId").AsInt32().Nullable()
            .WithColumn("TransactionDate").AsDateTime().NotNullable()
            .WithColumn("TransactionNo").AsString(100).NotNullable()
            .WithColumn("ReceiveFromAccountId").AsInt32().NotNullable()
            .WithColumn("Description").AsString(100).Nullable()
            .WithColumn("TaxId").AsInt32().Nullable()
            .WithColumn("Amount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("Memo").AsString(250).Nullable()
            .WithColumn("TotalAmount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}