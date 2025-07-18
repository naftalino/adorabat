using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bot.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDosProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ProductId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId1",
                table: "Orders",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductId1",
                table: "Orders",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
