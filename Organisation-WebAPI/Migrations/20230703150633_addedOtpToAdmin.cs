using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organisation_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addedOtpToAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "otp",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "otp",
                table: "Admins");
        }
    }
}
