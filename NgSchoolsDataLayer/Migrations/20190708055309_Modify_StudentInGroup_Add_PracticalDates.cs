using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_StudentInGroup_Add_PracticalDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PracticalEndDate",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "PracticalStartDate",
                table: "StudentGroups");

            migrationBuilder.AddColumn<DateTime>(
                name: "PracticalEndDate",
                table: "StudentsInGroups",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PracticalStartDate",
                table: "StudentsInGroups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PracticalEndDate",
                table: "StudentsInGroups");

            migrationBuilder.DropColumn(
                name: "PracticalStartDate",
                table: "StudentsInGroups");

            migrationBuilder.AddColumn<DateTime>(
                name: "PracticalEndDate",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PracticalStartDate",
                table: "StudentGroups",
                nullable: true);
        }
    }
}
