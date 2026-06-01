using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRequisition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditAndLoginLockoutFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlternativeDeviceNote",
                table: "Requests",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Laptops",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyExpiryDate",
                table: "Laptops",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEndDate",
                table: "Employees",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    EntityId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EntityType = table.Column<string>(type: "longtext", nullable: false),
                    Action = table.Column<string>(type: "longtext", nullable: false),
                    Changes = table.Column<string>(type: "longtext", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 29, 16, 344, DateTimeKind.Utc).AddTicks(3592), new DateTime(2026, 6, 1, 0, 29, 16, 344, DateTimeKind.Utc).AddTicks(3595) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 6, 1, 0, 29, 16, 344, DateTimeKind.Utc).AddTicks(4569), new DateTime(2026, 6, 1, 0, 29, 16, 344, DateTimeKind.Utc).AddTicks(4569) });

            migrationBuilder.CreateIndex(
                name: "IX_Laptops_AssignedToEmployeeId",
                table: "Laptops",
                column: "AssignedToEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Laptops_Employees_AssignedToEmployeeId",
                table: "Laptops",
                column: "AssignedToEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Laptops_Employees_AssignedToEmployeeId",
                table: "Laptops");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_Laptops_AssignedToEmployeeId",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "AlternativeDeviceNote",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "WarrantyExpiryDate",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "LockoutEndDate",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 31, 14, 26, 25, 764, DateTimeKind.Utc).AddTicks(3805), new DateTime(2026, 5, 31, 14, 26, 25, 764, DateTimeKind.Utc).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 31, 14, 26, 25, 764, DateTimeKind.Utc).AddTicks(5323), new DateTime(2026, 5, 31, 14, 26, 25, 764, DateTimeKind.Utc).AddTicks(5326) });
        }
    }
}
