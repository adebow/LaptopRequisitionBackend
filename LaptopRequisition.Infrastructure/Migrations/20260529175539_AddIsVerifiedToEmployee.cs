using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRequisition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsVerifiedToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Employees",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 29, 17, 55, 36, 849, DateTimeKind.Utc).AddTicks(7980), new DateTime(2026, 5, 29, 17, 55, 36, 849, DateTimeKind.Utc).AddTicks(7984) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 29, 16, 17, 5, 637, DateTimeKind.Utc).AddTicks(4464), new DateTime(2026, 5, 29, 16, 17, 5, 637, DateTimeKind.Utc).AddTicks(4468) });
        }
    }
}
