using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendenceApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Users_UserId1",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "Attendances",
                newName: "EmployeeId1");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_UserId1",
                table: "Attendances",
                newName: "IX_Attendances_EmployeeId1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Attendances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Employees_EmployeeId1",
                table: "Attendances",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Employees_EmployeeId1",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "EmployeeId1",
                table: "Attendances",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_EmployeeId1",
                table: "Attendances",
                newName: "IX_Attendances_UserId1");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Attendances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Users_UserId1",
                table: "Attendances",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
