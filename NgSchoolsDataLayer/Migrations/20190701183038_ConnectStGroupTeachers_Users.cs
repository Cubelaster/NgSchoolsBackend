using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class ConnectStGroupTeachers_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StudentGroupSubjectTeachers_TeacherId",
                table: "StudentGroupSubjectTeachers",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroupSubjectTeachers_AspNetUsers_TeacherId",
                table: "StudentGroupSubjectTeachers",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroupSubjectTeachers_AspNetUsers_TeacherId",
                table: "StudentGroupSubjectTeachers");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroupSubjectTeachers_TeacherId",
                table: "StudentGroupSubjectTeachers");
        }
    }
}
