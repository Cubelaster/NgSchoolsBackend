using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_Qualifications_Student_And_Active_EduProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Qualifications",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "EducationPrograms",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qualifications",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "EducationPrograms");
        }
    }
}
