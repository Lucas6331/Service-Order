using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OsDsII.api.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comentario_service_order_ServiceOrderId",
                table: "comentario");

            migrationBuilder.DropForeignKey(
                name: "FK_service_order_customer_CustomerId",
                table: "service_order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comentario",
                table: "comentario");

            migrationBuilder.RenameTable(
                name: "comentario",
                newName: "comments");

            migrationBuilder.RenameIndex(
                name: "IX_comentario_ServiceOrderId",
                table: "comments",
                newName: "IX_comments_ServiceOrderId");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OpeningDate",
                table: "service_order",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 3, 13, 14, 42, 8, 866, DateTimeKind.Unspecified).AddTicks(1763), new TimeSpan(0, -3, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SendDate",
                table: "comments",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 5, 17, 15, 52, 51, 111, DateTimeKind.Unspecified).AddTicks(7656), new TimeSpan(0, -3, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 3, 13, 14, 42, 8, 866, DateTimeKind.Unspecified).AddTicks(9418), new TimeSpan(0, -3, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_comments",
                table: "comments",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_service_order_ServiceOrderId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_service_order_customer_CustomerId",
                table: "service_order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comments",
                table: "comments");

            migrationBuilder.RenameTable(
                name: "comments",
                newName: "comentario");

            migrationBuilder.RenameIndex(
                name: "IX_comments_ServiceOrderId",
                table: "comentario",
                newName: "IX_comentario_ServiceOrderId");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "OpeningDate",
                table: "service_order",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 3, 13, 14, 42, 8, 866, DateTimeKind.Unspecified).AddTicks(1763), new TimeSpan(0, -3, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SendDate",
                table: "comentario",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2024, 3, 13, 14, 42, 8, 866, DateTimeKind.Unspecified).AddTicks(9418), new TimeSpan(0, -3, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2024, 5, 17, 15, 52, 51, 111, DateTimeKind.Unspecified).AddTicks(7656), new TimeSpan(0, -3, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_comentario",
                table: "comentario",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_comentario_service_order_ServiceOrderId",
                table: "comentario",
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
    }
}
