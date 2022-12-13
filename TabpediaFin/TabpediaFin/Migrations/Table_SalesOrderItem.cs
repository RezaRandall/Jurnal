﻿using FluentMigrator;

namespace TabpediaFin.Migrations;

[Migration(202212081406)]
public class Table_SalesOrderItem : Migration
{
    public override void Down()
    {

    }

    public override void Up()
    {
        Create.Table("SalesOrderItem")
            .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
            .WithColumn("TenantId").AsInt32().NotNullable()
            .WithColumn("TransId").AsInt32().NotNullable()
            .WithColumn("ItemId").AsInt32().NotNullable()
            .WithColumn("Quantity").AsInt32().NotNullable()
            .WithColumn("Price").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("TotalPrice").AsDecimal(18, 2).NotNullable().WithDefaultValue(0)
            .WithColumn("ItemUnitMeasureId").AsInt32().NotNullable()
            .WithColumn("CreatedUid").AsInt32().NotNullable()
            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
            .WithColumn("UpdatedUid").AsInt32().Nullable()
            .WithColumn("UpdatedUtc").AsDateTime().Nullable();
    }
}