using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Files_Teachers_Institution_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserDetailsId",
                table: "TeacherFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherFiles_UserDetailsId",
                table: "TeacherFiles",
                column: "UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherFiles_UserDetails_UserDetailsId",
                table: "TeacherFiles",
                column: "UserDetailsId",
                principalTable: "UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherFiles_UserDetails_UserDetailsId",
                table: "TeacherFiles");

            migrationBuilder.DropIndex(
                name: "IX_TeacherFiles_UserDetailsId",
                table: "TeacherFiles");

            migrationBuilder.DropColumn(
                name: "UserDetailsId",
                table: "TeacherFiles");
        }
    }
}
