using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_Missing_Props_On_StudentGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolYear",
                table: "StudentGroups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "SchoolYear",
                table: "StudentGroups");
        }
    }
}
