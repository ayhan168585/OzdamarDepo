using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzdamarDepo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Orders_OrderId1",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_OrderId1",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "Baskets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId1",
                table: "Baskets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_OrderId1",
                table: "Baskets",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Orders_OrderId1",
                table: "Baskets",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
