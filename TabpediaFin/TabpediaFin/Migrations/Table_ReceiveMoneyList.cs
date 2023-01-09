﻿using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202301021156)]
public class Table_ReceiveMoneyList : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("ReceiveMoneyList")
        .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
        .WithColumn("TenantId").AsInt32().NotNullable()
        .WithColumn("ReceiveMoneyId").AsInt32().NotNullable()
        .WithColumn("PriceIncludesTax").AsBoolean().NotNullable().WithDefaultValue(false)
        .WithColumn("ReceiveFromAccountId").AsInt32().NotNullable()
        .WithColumn("Description").AsString(100).Nullable()
        .WithColumn("TaxId").AsInt32().Nullable()
        .WithColumn("Amount").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
        .WithColumn("TransId").AsInt32().NotNullable().WithDefaultValue(0)
        .WithColumn("CreatedUid").AsInt32().NotNullable()
        .WithColumn("CreatedUtc").AsDateTime().NotNullable()
        .WithColumn("UpdatedUid").AsInt32().Nullable()
        .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}