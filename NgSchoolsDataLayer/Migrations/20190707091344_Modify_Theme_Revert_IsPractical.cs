using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Theme_Revert_IsPractical : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPracticalType",
                table: "Themes");

            migrationBuilder.AddColumn<bool>(
                name: "IsPracticalType",
                table: "Subjects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ClassTypes",
                table: "PlanDaySubjectThemes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PerfomingType",
                table: "PlanDaySubjectThemes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPracticalType",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ClassTypes",
                table: "PlanDaySubjectThemes");

            migrationBuilder.DropColumn(
                name: "PerfomingType",
                table: "PlanDaySubjectThemes");

            migrationBuilder.AddColumn<bool>(
                name: "IsPracticalType",
                table: "Themes",
                nullable: false,
                defaultValue: false);
        }
    }
}
