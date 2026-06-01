using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopRequisition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeEmployeeFksNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Employees_EmployeeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetTokens_Employees_EmployeeId",
                table: "PasswordResetTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employees_EmployeeId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRequests_Employees_EmployeeId",
                table: "ReturnRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "ReturnRequests",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "Requests",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "PasswordResetTokens",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "Notifications",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Employees_EmployeeId",
                table: "Notifications",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetTokens_Employees_EmployeeId",
                table: "PasswordResetTokens",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Employees_EmployeeId",
                table: "Requests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRequests_Employees_EmployeeId",
                table: "ReturnRequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Employees_EmployeeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetTokens_Employees_EmployeeId",
                table: "PasswordResetTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employees_EmployeeId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRequests_Employees_EmployeeId",
                table: "ReturnRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "ReturnRequests",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "Requests",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "PasswordResetTokens",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "Notifications",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 31, 13, 25, 20, 413, DateTimeKind.Utc).AddTicks(6273), new DateTime(2026, 5, 31, 13, 25, 20, 413, DateTimeKind.Utc).AddTicks(6275) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 5, 31, 13, 25, 20, 413, DateTimeKind.Utc).AddTicks(7403), new DateTime(2026, 5, 31, 13, 25, 20, 413, DateTimeKind.Utc).AddTicks(7405) });

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Employees_EmployeeId",
                table: "Notifications",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetTokens_Employees_EmployeeId",
                table: "PasswordResetTokens",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Employees_EmployeeId",
                table: "Requests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRequests_Employees_EmployeeId",
                table: "ReturnRequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
