using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirplaneProject.Migrations
{
    /// <inheritdoc />
    public partial class RenamedObjsToModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatReserve_Order_OrderId",
                table: "SeatReserve");

            migrationBuilder.DropIndex(
                name: "IX_SeatReserve_OrderId",
                table: "SeatReserve");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderModelId",
                table: "SeatReserve",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeatReserve_OrderModelId",
                table: "SeatReserve",
                column: "OrderModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatReserve_Order_OrderModelId",
                table: "SeatReserve",
                column: "OrderModelId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatReserve_Order_OrderModelId",
                table: "SeatReserve");

            migrationBuilder.DropIndex(
                name: "IX_SeatReserve_OrderModelId",
                table: "SeatReserve");

            migrationBuilder.DropColumn(
                name: "OrderModelId",
                table: "SeatReserve");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReserve_OrderId",
                table: "SeatReserve",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatReserve_Order_OrderId",
                table: "SeatReserve",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
