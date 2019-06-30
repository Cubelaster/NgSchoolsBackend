using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_GoverningCouncil_ChangeRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_GoverningCouncil_GoverningCouncilId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_GoverningCouncilId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "GoverningCouncilId",
                table: "Institution");

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "GoverningCouncil",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GoverningCouncil_InstitutionId",
                table: "GoverningCouncil",
                column: "InstitutionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GoverningCouncil_Institution_InstitutionId",
                table: "GoverningCouncil",
                column: "InstitutionId",
                principalTable: "Institution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoverningCouncil_Institution_InstitutionId",
                table: "GoverningCouncil");

            migrationBuilder.DropIndex(
                name: "IX_GoverningCouncil_InstitutionId",
                table: "GoverningCouncil");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "GoverningCouncil");

            migrationBuilder.AddColumn<int>(
                name: "GoverningCouncilId",
                table: "Institution",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_GoverningCouncilId",
                table: "Institution",
                column: "GoverningCouncilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_GoverningCouncil_GoverningCouncilId",
                table: "Institution",
                column: "GoverningCouncilId",
                principalTable: "GoverningCouncil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
