using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_SubjectTheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LearningOucomes",
                table: "Themes",
                newName: "LearningOutcomes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LearningOutcomes",
                table: "Themes",
                newName: "LearningOucomes");
        }
    }
}
