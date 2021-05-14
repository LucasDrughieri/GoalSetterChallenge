using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class LogicalDeleteFieldAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Clients");
        }
    }
}
