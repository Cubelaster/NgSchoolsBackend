using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_EducationProgram_Connect_Plan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_PlanId",
                table: "EducationPrograms",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationPrograms_Plans_PlanId",
                table: "EducationPrograms",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationPrograms_Plans_PlanId",
                table: "EducationPrograms");

            migrationBuilder.DropIndex(
                name: "IX_EducationPrograms_PlanId",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "EducationPrograms");
        }
    }
}
