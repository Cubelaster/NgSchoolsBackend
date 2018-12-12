using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Exam_Commission_Add_Db_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "UserExamCommission",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "UserExamCommission",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserExamCommission",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ExamCommissions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "ExamCommissions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ExamCommissions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "UserExamCommission");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "UserExamCommission");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserExamCommission");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ExamCommissions");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "ExamCommissions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ExamCommissions");
        }
    }
}
