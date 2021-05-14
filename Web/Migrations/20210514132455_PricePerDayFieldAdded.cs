using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class PricePerDayFieldAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PricePerDay",
                table: "Vehicles",
                type: "float",
                nullable: false,
                defaultValue: 1.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerDay",
                table: "Vehicles");
        }
    }
}
