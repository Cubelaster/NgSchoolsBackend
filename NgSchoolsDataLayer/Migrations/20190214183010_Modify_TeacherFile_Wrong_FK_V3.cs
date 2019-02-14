using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_TeacherFile_Wrong_FK_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherFiles_UserDetails_UserDetailsId",
                table: "TeacherFiles");

            migrationBuilder.DropColumn(
                name: "UserDetailId",
                table: "TeacherFiles");

            migrationBuilder.AlterColumn<int>(
                name: "UserDetailsId",
                table: "TeacherFiles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherFiles_UserDetails_UserDetailsId",
                table: "TeacherFiles",
                column: "UserDetailsId",
                principalTable: "UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherFiles_UserDetails_UserDetailsId",
                table: "TeacherFiles");

            migrationBuilder.AlterColumn<int>(
                name: "UserDetailsId",
                table: "TeacherFiles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "UserDetailId",
                table: "TeacherFiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherFiles_UserDetails_UserDetailsId",
                table: "TeacherFiles",
                column: "UserDetailsId",
                principalTable: "UserDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
