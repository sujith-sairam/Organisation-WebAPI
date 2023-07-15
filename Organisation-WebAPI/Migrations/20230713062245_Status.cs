using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organisation_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Products_ProductID",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "TaskStatus",
                table: "EmployeeTasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Products_ProductID",
                table: "Employees",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Products_ProductID",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "TaskStatus",
                table: "EmployeeTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Products_ProductID",
                table: "Employees",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
