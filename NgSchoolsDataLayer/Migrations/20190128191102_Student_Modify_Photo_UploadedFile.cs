using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Student_Modify_Photo_UploadedFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_PhotoId",
                table: "Students",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_UploadedFiles_PhotoId",
                table: "Students",
                column: "PhotoId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_UploadedFiles_PhotoId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_PhotoId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Students",
                nullable: true);
        }
    }
}
