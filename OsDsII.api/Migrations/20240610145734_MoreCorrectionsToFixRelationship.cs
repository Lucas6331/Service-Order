using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsDsII.api.Migrations
{
    /// <inheritdoc />
    public partial class MoreCorrectionsToFixRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_service_order_ServiceOrderId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_service_order_customer_CustomerId",
                table: "service_order");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OpeningDate",
                table: "service_order",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 6, 10, 11, 57, 34, 155, DateTimeKind.Unspecified).AddTicks(3234), new TimeSpan(0, -3, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SendDate",
                table: "comments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 6, 10, 11, 57, 34, 155, DateTimeKind.Unspecified).AddTicks(6779), new TimeSpan(0, -3, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 5, 17, 15, 52, 51, 111, DateTimeKind.Unspecified).AddTicks(7656), new TimeSpan(0, -3, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                name: "FK_comments_service_order_ServiceOrderId",
                table: "comments",
                column: "ServiceOrderId",
                principalTable: "service_order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_service_order_customer_CustomerId",
                table: "service_order",
                column: "CustomerId",
                principalTable: "customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_service_order_ServiceOrderId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_service_order_customer_CustomerId",
                table: "service_order");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OpeningDate",
                table: "service_order",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 6, 10, 11, 57, 34, 155, DateTimeKind.Unspecified).AddTicks(3234), new TimeSpan(0, -3, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SendDate",
                table: "comments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 5, 17, 15, 52, 51, 111, DateTimeKind.Unspecified).AddTicks(7656), new TimeSpan(0, -3, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 6, 10, 11, 57, 34, 155, DateTimeKind.Unspecified).AddTicks(6779), new TimeSpan(0, -3, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                name: "FK_comments_service_order_ServiceOrderId",
                table: "comments",
                column: "ServiceOrderId",
                principalTable: "service_order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_service_order_customer_CustomerId",
                table: "service_order",
                column: "CustomerId",
                principalTable: "customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
