using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRequisition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedToEmployeeIdToLaptop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedToEmployeeId",
                table: "Laptops",
                type: "char(36)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5212), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5215) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5222), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5223) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5225), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5226) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5228), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5229) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5231), new DateTime(2026, 5, 24, 23, 49, 5, 647, DateTimeKind.Utc).AddTicks(5232) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToEmployeeId",
                table: "Laptops");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9253), new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9256) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9260), new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9261) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9267), new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9267) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9271), new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9272) });

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9274), new DateTime(2026, 5, 24, 13, 49, 38, 839, DateTimeKind.Utc).AddTicks(9275) });
        }
    }
}
