using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SA.EntityFramework.Migrations
{
    public partial class TimestampToDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "jera-trading");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfFirstRegistration",
                table: "records",
                type: "DateTime",                
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Stk",
                table: "records",
                type: "DateTime",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidFrom",
                table: "records",
                type: "DateTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidTo",
                table: "records",
                type: "DateTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "records",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP"
                );

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "users",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "useractivations",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime?>(
                name: "Verified",
                table: "useractivations",
                type: "DateTime",
                defaultValueSql: "NULL",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "files",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "gdprrecords",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "customers",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "countries",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "bids",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "addresses",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                table: "auctions",
                type: "DateTime",
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidFrom",
                table: "auctions",
                type: "DateTime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidTo",
                table: "auctions",
                type: "DateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
