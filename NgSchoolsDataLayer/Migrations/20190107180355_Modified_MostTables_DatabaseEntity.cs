using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modified_MostTables_DatabaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StudentsInGroups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "StudentsInGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StudentsInGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Students",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Students",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StudentGroups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StudentGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Institution",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Institution",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Institution",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "EducationPrograms",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EducationPrograms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "EducationLevels",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "EducationLevels",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EducationLevels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "EducationGroups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "EducationGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EducationGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ClassTypes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "ClassTypes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ClassTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ClassLocations",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "ClassLocations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ClassLocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StudentsInGroups");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "StudentsInGroups");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentsInGroups");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "EducationGroups");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "EducationGroups");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EducationGroups");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ClassTypes");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "ClassTypes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ClassTypes");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BusinessPartners");
        }
    }
}
