using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_TeacherFile_Wrong_FK_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherFiles_AspNetUsers_TeacherId",
                table: "TeacherFiles");

            migrationBuilder.DropIndex(
                name: "IX_TeacherFiles_TeacherId",
                table: "TeacherFiles");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "TeacherFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "TeacherFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherFiles_TeacherId",
                table: "TeacherFiles",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherFiles_AspNetUsers_TeacherId",
                table: "TeacherFiles",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
