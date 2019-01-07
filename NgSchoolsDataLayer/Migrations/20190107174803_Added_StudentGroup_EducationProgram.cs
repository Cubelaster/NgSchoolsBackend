using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Added_StudentGroup_EducationProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Students",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Students",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "EducationPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ShorthandName = table.Column<string>(nullable: true),
                    ProgramDuration = table.Column<double>(nullable: true),
                    ProgramDurationTextual = table.Column<string>(nullable: true),
                    ProgramDurationDays = table.Column<double>(nullable: true),
                    FinishedSchool = table.Column<string>(nullable: true),
                    ProgramDate = table.Column<DateTime>(nullable: true),
                    TheoreticalClassesDuration = table.Column<double>(nullable: true),
                    PracticalClassesDuration = table.Column<double>(nullable: true),
                    ApprovalClass = table.Column<string>(nullable: true),
                    UrNumber = table.Column<string>(nullable: true),
                    ComplexityLevel = table.Column<string>(nullable: true),
                    ProgramJustifiability = table.Column<string>(nullable: true),
                    EnrollmentConditions = table.Column<string>(nullable: true),
                    WorkingEnvironment = table.Column<string>(nullable: true),
                    ProgramCompetencies = table.Column<string>(nullable: true),
                    PerformingWay = table.Column<string>(nullable: true),
                    KnoweledgeVerification = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CredentialDate = table.Column<DateTime>(nullable: false),
                    FirstExamDate = table.Column<DateTime>(nullable: false),
                    SecondExamDate = table.Column<DateTime>(nullable: false),
                    ProgramId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGroups_EducationPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentsInGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<int>(nullable: false),
                    StudentGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsInGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentsInGroups_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsInGroups_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_ProgramId",
                table: "StudentGroups",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsInGroups_StudentGroupId",
                table: "StudentsInGroups",
                column: "StudentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsInGroups_StudentId",
                table: "StudentsInGroups",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsInGroups");

            migrationBuilder.DropTable(
                name: "StudentGroups");

            migrationBuilder.DropTable(
                name: "EducationPrograms");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Students",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Students",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
