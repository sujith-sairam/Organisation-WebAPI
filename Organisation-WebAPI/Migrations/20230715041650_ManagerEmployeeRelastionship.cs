using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organisation_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class ManagerEmployeeRelastionship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerID",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerID",
                table: "Employees",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Managers_ManagerID",
                table: "Employees",
                column: "ManagerID",
                principalTable: "Managers",
                principalColumn: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Managers_ManagerID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ManagerID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ManagerID",
                table: "Employees");
        }
    }
}
