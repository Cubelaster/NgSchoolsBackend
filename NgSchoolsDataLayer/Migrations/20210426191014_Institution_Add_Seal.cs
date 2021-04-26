using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Institution_Add_Seal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SealId",
                table: "Institution",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_SealId",
                table: "Institution",
                column: "SealId");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_UploadedFiles_SealId",
                table: "Institution",
                column: "SealId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_UploadedFiles_SealId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_SealId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "SealId",
                table: "Institution");
        }
    }
}
