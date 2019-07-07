using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_StudentGroup_Add_PracticalDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PracticalEndDate",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PracticalStartDate",
                table: "StudentGroups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PracticalEndDate",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "PracticalStartDate",
                table: "StudentGroups");
        }
    }
}
