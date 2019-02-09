using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class ModifyStudentRegister_DatabaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StudentRegisters",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "StudentRegisters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StudentRegisters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StudentRegisterEntryPrints",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "StudentRegisterEntryPrints",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StudentRegisterEntryPrints",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StudentRegisterEntries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "StudentRegisterEntries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StudentRegisterEntries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StudentRegisters");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "StudentRegisters");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentRegisters");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StudentRegisterEntryPrints");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "StudentRegisterEntryPrints");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentRegisterEntryPrints");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StudentRegisterEntries");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "StudentRegisterEntries");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentRegisterEntries");
        }
    }
}
