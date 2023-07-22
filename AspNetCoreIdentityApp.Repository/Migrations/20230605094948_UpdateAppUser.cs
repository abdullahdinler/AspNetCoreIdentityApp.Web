using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreIdentityApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDay",
                table: "AspNetRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Gender",
                table: "AspNetRoles",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDay",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "AspNetRoles");
        }
    }
}
