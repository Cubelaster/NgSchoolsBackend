using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_GoverningCouncil_DbEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "GoverningCouncil",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "GoverningCouncil",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GoverningCouncil",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "GoverningCouncil");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "GoverningCouncil");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GoverningCouncil");
        }
    }
}
