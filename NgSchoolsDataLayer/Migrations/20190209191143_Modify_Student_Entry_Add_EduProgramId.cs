using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Student_Entry_Add_EduProgramId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EducationProgramId",
                table: "StudentRegisterEntries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegisterEntries_EducationProgramId",
                table: "StudentRegisterEntries",
                column: "EducationProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRegisterEntries_EducationPrograms_EducationProgramId",
                table: "StudentRegisterEntries",
                column: "EducationProgramId",
                principalTable: "EducationPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRegisterEntries_EducationPrograms_EducationProgramId",
                table: "StudentRegisterEntries");

            migrationBuilder.DropIndex(
                name: "IX_StudentRegisterEntries_EducationProgramId",
                table: "StudentRegisterEntries");

            migrationBuilder.DropColumn(
                name: "EducationProgramId",
                table: "StudentRegisterEntries");
        }
    }
}
