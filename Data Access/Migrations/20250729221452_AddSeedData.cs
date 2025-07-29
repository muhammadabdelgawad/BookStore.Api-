using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data_Access.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CatName", "CatOrder", "CreatedDate", "MarkedAsDeleted" },
                values: new object[,]
                {
                    { 1, "Electronics", 1, new DateTime(2025, 7, 29, 22, 14, 51, 346, DateTimeKind.Utc).AddTicks(6501), false },
                    { 2, "Books", 2, new DateTime(2025, 7, 29, 22, 14, 51, 346, DateTimeKind.Utc).AddTicks(6503), false },
                    { 3, "Computers", 3, new DateTime(2025, 7, 29, 22, 14, 51, 346, DateTimeKind.Utc).AddTicks(6505), false },
                    { 4, "Phones", 4, new DateTime(2025, 7, 29, 22, 14, 51, 346, DateTimeKind.Utc).AddTicks(6506), false }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "BookPrice", "Title" },
                values: new object[,]
                {
                    { 1, "ahmed", 1, "test01", 450.0, "Laptop" },
                    { 2, "muhammad", 3, "test02", 150.0, "Mobile" },
                    { 3, "kareem", 3, "test03", 320.0, "PC" },
                    { 4, "mido", 3, "test04", 630.0, "PC" },
                    { 5, "hamda", 2, "test05", 240.0, "BOOK" },
                    { 6, "mahmoud", 2, "test06", 50.0, "BOOK" },
                    { 7, "abdullah", 1, "test07", 10.0, "Mobile" },
                    { 8, "alaa", 1, "test08", 100.0, "Mobile" },
                    { 9, "abdulrahman", 2, "test09", 270.0, "Laptop" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
