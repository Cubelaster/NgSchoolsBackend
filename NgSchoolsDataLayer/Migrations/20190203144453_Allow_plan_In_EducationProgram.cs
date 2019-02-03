using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Allow_plan_In_EducationProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plans_EducationProgramId",
                table: "Plans");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_EducationProgramId",
                table: "Plans",
                column: "EducationProgramId",
                unique: true,
                filter: "[EducationProgramId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plans_EducationProgramId",
                table: "Plans");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_EducationProgramId",
                table: "Plans",
                column: "EducationProgramId");
        }
    }
}
