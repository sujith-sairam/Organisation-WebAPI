using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organisation_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdminChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Otp",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "OtpExpiration",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "OtpResendCount",
                table: "Admins",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Users_UserId",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_Admins_UserId",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Admins",
                newName: "OtpResendCount");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Admins",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Otp",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "OtpExpiration",
                table: "Admins",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Admins",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Admins",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
