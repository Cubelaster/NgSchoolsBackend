using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Student_Files_DatabaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StudentFiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "StudentFiles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StudentFiles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StudentFiles");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "StudentFiles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentFiles");
        }
    }
}
