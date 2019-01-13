using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Tables_EducationLevel_Student : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FinishedSchool",
                table: "Students",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressCountry",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Couob",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Dob",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IdCard",
                table: "Students",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Mob",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CognitiveSkills",
                table: "EducationLevels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KnowledgeBase",
                table: "EducationLevels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressCountry",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Couob",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Dob",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IdCard",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Mob",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CognitiveSkills",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "KnowledgeBase",
                table: "EducationLevels");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinishedSchool",
                table: "Students",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
