using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Theme_Add_IsPracticalType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPracticalType",
                table: "Subjects");

            migrationBuilder.AddColumn<bool>(
                name: "IsPracticalType",
                table: "Themes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPracticalType",
                table: "Themes");

            migrationBuilder.AddColumn<bool>(
                name: "IsPracticalType",
                table: "Subjects",
                nullable: false,
                defaultValue: false);
        }
    }
}
