using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LaptopRequisition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LaptopEntityUpdateAndSingleDepartmentSeed_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "Specifications",
                table: "Laptops");

            migrationBuilder.AddColumn<string>(
                name: "AssetTag",
                table: "Laptops",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "Laptops",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Laptops",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Laptops",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OperatingSystem",
                table: "Laptops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Processor",
                table: "Laptops",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RAM",
                table: "Laptops",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ScreenSize",
                table: "Laptops",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Storage",
                table: "Laptops",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 29, 12, 12, 49, 451, DateTimeKind.Utc).AddTicks(8139), new DateTime(2026, 5, 29, 12, 12, 49, 451, DateTimeKind.Utc).AddTicks(8142) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetTag",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "Processor",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "RAM",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "ScreenSize",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "Storage",
                table: "Laptops");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Laptops",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Specifications",
                table: "Laptops",
                type: "longtext",
                nullable: false);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5231), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5232) });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5212), "Human Resources", new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5215) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5222), "Management", new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5223) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5225), "Finance", new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5226) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5228), "Corporate Communications", new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5229) }
                });
        }
    }
}
