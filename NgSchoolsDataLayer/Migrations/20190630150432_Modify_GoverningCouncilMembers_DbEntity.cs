using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_GoverningCouncilMembers_DbEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "GoverningCouncilMember",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "GoverningCouncilMember",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GoverningCouncilMember",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "GoverningCouncilMember");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "GoverningCouncilMember");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GoverningCouncilMember");
        }
    }
}
