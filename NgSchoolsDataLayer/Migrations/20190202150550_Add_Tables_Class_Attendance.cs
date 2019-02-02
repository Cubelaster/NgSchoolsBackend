using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_Tables_Class_Attendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Subjects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Subjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StudentGroupSubjectTeachers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "StudentGroupSubjectTeachers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StudentGroupSubjectTeachers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "DiaryStudentGroups",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "DiaryStudentGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "DiaryStudentGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StudentGroupClassAttendances",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StudentGroupId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroupClassAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGroupClassAttendances_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentClassAttendances",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Attendance = table.Column<bool>(nullable: false),
                    StudentGroupClassAttendanceId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClassAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentClassAttendances_StudentGroupClassAttendances_StudentGroupClassAttendanceId",
                        column: x => x.StudentGroupClassAttendanceId,
                        principalTable: "StudentGroupClassAttendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentClassAttendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassAttendances_StudentGroupClassAttendanceId",
                table: "StudentClassAttendances",
                column: "StudentGroupClassAttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassAttendances_StudentId",
                table: "StudentClassAttendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroupClassAttendances_StudentGroupId",
                table: "StudentGroupClassAttendances",
                column: "StudentGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentClassAttendances");

            migrationBuilder.DropTable(
                name: "StudentGroupClassAttendances");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StudentGroupSubjectTeachers");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "StudentGroupSubjectTeachers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StudentGroupSubjectTeachers");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "DiaryStudentGroups");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "DiaryStudentGroups");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DiaryStudentGroups");
        }
    }
}
