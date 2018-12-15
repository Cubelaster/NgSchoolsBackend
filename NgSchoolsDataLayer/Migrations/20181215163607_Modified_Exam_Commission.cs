using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modified_Exam_Commission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExamCommission_ExamCommissions_ExamCommissionId",
                table: "UserExamCommission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExamCommission_AspNetUsers_UserId",
                table: "UserExamCommission");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserExamCommission",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExamCommissionId",
                table: "UserExamCommission",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamCommission_ExamCommissions_ExamCommissionId",
                table: "UserExamCommission",
                column: "ExamCommissionId",
                principalTable: "ExamCommissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamCommission_AspNetUsers_UserId",
                table: "UserExamCommission",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExamCommission_ExamCommissions_ExamCommissionId",
                table: "UserExamCommission");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExamCommission_AspNetUsers_UserId",
                table: "UserExamCommission");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserExamCommission",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<int>(
                name: "ExamCommissionId",
                table: "UserExamCommission",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamCommission_ExamCommissions_ExamCommissionId",
                table: "UserExamCommission",
                column: "ExamCommissionId",
                principalTable: "ExamCommissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamCommission_AspNetUsers_UserId",
                table: "UserExamCommission",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
