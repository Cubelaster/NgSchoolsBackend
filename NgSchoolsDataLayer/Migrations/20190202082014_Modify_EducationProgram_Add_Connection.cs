using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_EducationProgram_Add_Connection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EducationGroupId",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_EducationGroupId",
                table: "EducationPrograms",
                column: "EducationGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationPrograms_EducationGroups_EducationGroupId",
                table: "EducationPrograms",
                column: "EducationGroupId",
                principalTable: "EducationGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationPrograms_EducationGroups_EducationGroupId",
                table: "EducationPrograms");

            migrationBuilder.DropIndex(
                name: "IX_EducationPrograms_EducationGroupId",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "EducationGroupId",
                table: "EducationPrograms");
        }
    }
}
