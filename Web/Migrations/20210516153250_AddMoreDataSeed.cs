using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class AddMoreDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Active", "Name", "Phone" },
                values: new object[,]
                {
                    { 6, true, "Bob Patiño", "66666" },
                    { 7, false, "Jefe Gorgory", "777777" },
                    { 8, true, "Moe", "888888" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Active", "Brand", "PricePerDay", "Year" },
                values: new object[,]
                {
                    { 7, true, "Renault", 10.0, 2020 },
                    { 8, true, "Audi", 20.0, 2020 },
                    { 9, false, "Ford", 15.0, 2020 },
                    { 10, true, "Fiat", 10.0, 2020 },
                    { 11, true, "BMW", 25.0, 2020 },
                    { 12, true, "volkswagen", 10.0, 2020 }
                });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "ClientId", "EndDate", "Price", "StartDate", "Status", "VehicleId" },
                values: new object[] { 4, 3, new DateTime(2021, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 70.0, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 7 });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "ClientId", "EndDate", "Price", "StartDate", "Status", "VehicleId" },
                values: new object[] { 5, 4, new DateTime(2021, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 20.0, new DateTime(2021, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 10 });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "ClientId", "EndDate", "Price", "StartDate", "Status", "VehicleId" },
                values: new object[] { 6, 8, new DateTime(2021, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 50.0, new DateTime(2021, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 12 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
