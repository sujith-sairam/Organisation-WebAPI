using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organisation_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class CapitalChangeAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "otp",
                table: "Admins",
                newName: "Otp");

            migrationBuilder.RenameColumn(
                name: "isVerified",
                table: "Admins",
                newName: "IsVerified");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Otp",
                table: "Admins",
                newName: "otp");

            migrationBuilder.RenameColumn(
                name: "IsVerified",
                table: "Admins",
                newName: "isVerified");
        }
    }
}
