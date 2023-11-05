using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirplaneProject.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderActiveness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Order",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Order");
        }
    }
}
