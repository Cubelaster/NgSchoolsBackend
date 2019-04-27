using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_StudentRegisterEntry_AddExamDateNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamDate",
                table: "StudentRegisters");

            migrationBuilder.DropColumn(
                name: "ExamDateNumber",
                table: "StudentRegisters");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EntryDate",
                table: "StudentRegisterEntries",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExamDate",
                table: "StudentRegisterEntries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamDateNumber",
                table: "StudentRegisterEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamDate",
                table: "StudentRegisterEntries");

            migrationBuilder.DropColumn(
                name: "ExamDateNumber",
                table: "StudentRegisterEntries");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExamDate",
                table: "StudentRegisters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamDateNumber",
                table: "StudentRegisters",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EntryDate",
                table: "StudentRegisterEntries",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
