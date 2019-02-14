using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Files_Teachers_Institution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "TeacherFiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "TeacherFiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TeacherFiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "InstitutionFiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "InstitutionFiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "InstitutionFiles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "TeacherFiles");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "TeacherFiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TeacherFiles");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "InstitutionFiles");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "InstitutionFiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "InstitutionFiles");
        }
    }
}
