using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppTest.Migrations
{
    /// <inheritdoc />
    public partial class Renamedcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Bookings");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
