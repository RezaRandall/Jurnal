﻿//using FluentMigrator;

//namespace TabpediaFin.Migrations;

//[Migration(202211241909)]
//public class Table_SysAccountGroup : Migration
//{
//    public override void Down()
//    {

//    }

//    public override void Up()
//    {
//        Create.Table("SysAccountGroup")
//            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey()
//            .WithColumn("Name").AsString(250).NotNullable()
//            .WithColumn("Description").AsString(250).Nullable()
//            .WithColumn("CreatedUid").AsInt32().NotNullable()
//            .WithColumn("CreatedUtc").AsDateTime().NotNullable()
//            .WithColumn("UpdatedUid").AsInt32().Nullable()
//            .WithColumn("UpdatedUtc").AsDateTime().Nullable();

//    }
//}
