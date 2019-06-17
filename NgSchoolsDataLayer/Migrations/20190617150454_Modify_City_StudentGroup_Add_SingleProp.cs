using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_City_StudentGroup_Add_SingleProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EducationGroupMark",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PoBoxNumber",
                table: "Cities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EducationGroupMark",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "PoBoxNumber",
                table: "Cities");
        }
    }
}
