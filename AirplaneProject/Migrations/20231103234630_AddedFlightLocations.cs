using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AirplaneProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedFlightLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartureLocation",
                table: "Flight",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DestinationLocation",
                table: "Flight",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartureLocation",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "DestinationLocation",
                table: "Flight");
        }
    }
}
