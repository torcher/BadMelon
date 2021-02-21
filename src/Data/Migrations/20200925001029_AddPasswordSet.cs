using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BadMelon.Data.Migrations
{
    public partial class AddPasswordSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationCreate",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationCreated",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPasswordSet",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationCreated",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsPasswordSet",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationCreate",
                table: "AspNetUsers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
