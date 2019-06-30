using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_StudentGroup_ExamCommission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommissionRole",
                table: "UserExamCommission",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PracticalExamCommissionId",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PracticalExamFirstDate",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PracticalExamSecondDate",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_PracticalExamCommissionId",
                table: "StudentGroups",
                column: "PracticalExamCommissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_ExamCommissions_PracticalExamCommissionId",
                table: "StudentGroups",
                column: "PracticalExamCommissionId",
                principalTable: "ExamCommissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_ExamCommissions_PracticalExamCommissionId",
                table: "StudentGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_PracticalExamCommissionId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "CommissionRole",
                table: "UserExamCommission");

            migrationBuilder.DropColumn(
                name: "PracticalExamCommissionId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "PracticalExamFirstDate",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "PracticalExamSecondDate",
                table: "StudentGroups");
        }
    }
}
