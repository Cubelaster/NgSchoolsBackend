using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_EduProgram_Files_DbEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "EducationProgramFiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "EducationProgramFiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EducationProgramFiles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "EducationProgramFiles");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "EducationProgramFiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EducationProgramFiles");
        }
    }
}
