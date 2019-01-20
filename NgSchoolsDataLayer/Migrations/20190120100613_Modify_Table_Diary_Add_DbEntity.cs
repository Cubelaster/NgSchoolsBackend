using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Table_Diary_Add_DbEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Diaries",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Diaries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Diaries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Diaries");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Diaries");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Diaries");
        }
    }
}
