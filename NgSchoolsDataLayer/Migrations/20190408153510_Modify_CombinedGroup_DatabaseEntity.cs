using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_CombinedGroup_DatabaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "CombinedGroup",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "CombinedGroup",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CombinedGroup",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "CombinedGroup");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "CombinedGroup");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CombinedGroup");
        }
    }
}
