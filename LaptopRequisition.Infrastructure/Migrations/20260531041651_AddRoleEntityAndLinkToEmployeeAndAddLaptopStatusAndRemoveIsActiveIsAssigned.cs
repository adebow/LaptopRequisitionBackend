using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRequisition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleEntityAndLinkToEmployeeAndAddLaptopStatusAndRemoveIsActiveIsAssigned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "IsAssigned",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Laptops",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Employees",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 31, 4, 16, 48, 885, DateTimeKind.Utc).AddTicks(3324), new DateTime(2026, 5, 31, 4, 16, 48, 885, DateTimeKind.Utc).AddTicks(3326) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 5, 31, 4, 16, 48, 885, DateTimeKind.Utc).AddTicks(3968), "Administrator with full access", "Admin", new DateTime(2026, 5, 31, 4, 16, 48, 885, DateTimeKind.Utc).AddTicks(3969) });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Employees");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Laptops",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigned",
                table: "Laptops",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Employees",
                type: "longtext",
                nullable: false);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 29, 17, 55, 36, 849, DateTimeKind.Utc).AddTicks(7980), new DateTime(2026, 5, 29, 17, 55, 36, 849, DateTimeKind.Utc).AddTicks(7984) });
        }
    }
}
