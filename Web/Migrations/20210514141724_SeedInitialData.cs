using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Active", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, true, "Homero", "11111" },
                    { 2, true, "Bart", "22222" },
                    { 3, true, "Lisa", "33333" },
                    { 4, true, "Marge", "44444" },
                    { 5, false, "Maggie", "55555" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Active", "Brand", "PricePerDay", "Year" },
                values: new object[,]
                {
                    { 1, true, "Renault", 10.0, 2021 },
                    { 2, true, "Audi", 20.0, 2021 },
                    { 3, true, "Ford", 15.0, 2021 },
                    { 4, false, "Fiat", 10.0, 2021 },
                    { 5, true, "BMW", 25.0, 2021 },
                    { 6, false, "volkswagen", 10.0, 2021 }
                });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "ClientId", "EndDate", "Price", "StartDate", "VehicleId" },
                values: new object[] { 1, 1, new DateTime(2021, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 70.0, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "ClientId", "EndDate", "Price", "StartDate", "VehicleId" },
                values: new object[] { 3, 2, new DateTime(2021, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 70.0, new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "Id", "ClientId", "EndDate", "Price", "StartDate", "VehicleId" },
                values: new object[] { 2, 1, new DateTime(2021, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 140.0, new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
