using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Location_Tables_DbEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Regions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Regions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Regions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Countries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Countries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Countries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Cities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Cities",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Cities",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cities");
        }
    }
}
