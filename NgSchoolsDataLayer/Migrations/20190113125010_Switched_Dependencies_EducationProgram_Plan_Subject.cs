using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Switched_Dependencies_EducationProgram_Plan_Subject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Themes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "EducationProgramId",
                table: "Subjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EducationPogramId",
                table: "Plans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EducationProgramId",
                table: "Plans",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "UI_ThemeName",
                table: "Themes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_EducationProgramId",
                table: "Subjects",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_EducationProgramId",
                table: "Plans",
                column: "EducationProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_EducationPrograms_EducationProgramId",
                table: "Plans",
                column: "EducationProgramId",
                principalTable: "EducationPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_EducationPrograms_EducationProgramId",
                table: "Subjects",
                column: "EducationProgramId",
                principalTable: "EducationPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_EducationPrograms_EducationProgramId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_EducationPrograms_EducationProgramId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "UI_ThemeName",
                table: "Themes");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_EducationProgramId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Plans_EducationProgramId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "EducationProgramId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "EducationPogramId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "EducationProgramId",
                table: "Plans");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Themes",
                nullable: false,
                oldClrType: typeof(string));

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
    }
}
