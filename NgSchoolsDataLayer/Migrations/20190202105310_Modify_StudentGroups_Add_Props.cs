using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_StudentGroups_Add_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EducationLeaderId",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrolmentDate",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamCommissionId",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_EducationLeaderId",
                table: "StudentGroups",
                column: "EducationLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_ExamCommissionId",
                table: "StudentGroups",
                column: "ExamCommissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_AspNetUsers_EducationLeaderId",
                table: "StudentGroups",
                column: "EducationLeaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_ExamCommissions_ExamCommissionId",
                table: "StudentGroups",
                column: "ExamCommissionId",
                principalTable: "ExamCommissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_AspNetUsers_EducationLeaderId",
                table: "StudentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_ExamCommissions_ExamCommissionId",
                table: "StudentGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_EducationLeaderId",
                table: "StudentGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_ExamCommissionId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "EducationLeaderId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "EnrolmentDate",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "ExamCommissionId",
                table: "StudentGroups");
        }
    }
}
