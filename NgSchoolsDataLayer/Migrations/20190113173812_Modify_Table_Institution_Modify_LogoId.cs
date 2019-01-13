using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Table_Institution_Modify_LogoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_UploadedFiles_LogoId1",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_LogoId1",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "LogoId1",
                table: "Institution");

            migrationBuilder.AlterColumn<int>(
                name: "LogoId",
                table: "Institution",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_LogoId",
                table: "Institution",
                column: "LogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_UploadedFiles_LogoId",
                table: "Institution",
                column: "LogoId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_UploadedFiles_LogoId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_LogoId",
                table: "Institution");

            migrationBuilder.AlterColumn<string>(
                name: "LogoId",
                table: "Institution",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LogoId1",
                table: "Institution",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_LogoId1",
                table: "Institution",
                column: "LogoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_UploadedFiles_LogoId1",
                table: "Institution",
                column: "LogoId1",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
