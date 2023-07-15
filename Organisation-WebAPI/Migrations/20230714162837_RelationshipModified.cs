using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organisation_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Managers",
                newName: "ManagerID");

            migrationBuilder.AddColumn<int>(
                name: "ManagerID1",
                table: "Managers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Managers_ManagerID1",
                table: "Managers",
                column: "ManagerID1");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerID",
                table: "Employees",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Managers_ManagerID",
                table: "Employees",
                column: "ManagerID",
                principalTable: "Managers",
                principalColumn: "ManagerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Managers_ManagerID1",
                table: "Managers",
                column: "ManagerID1",
                principalTable: "Managers",
                principalColumn: "ManagerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Managers_ManagerID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Managers_ManagerID1",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Managers_ManagerID1",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ManagerID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ManagerID1",
                table: "Managers");

            migrationBuilder.RenameColumn(
                name: "ManagerID",
                table: "Managers",
                newName: "ManagerId");
        }
    }
}
