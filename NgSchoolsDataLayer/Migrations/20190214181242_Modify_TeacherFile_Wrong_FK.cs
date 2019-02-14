using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_TeacherFile_Wrong_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherFiles_AspNetUsers_TeacherId",
                table: "TeacherFiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                table: "TeacherFiles",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<int>(
                name: "UserDetailId",
                table: "TeacherFiles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EducationProgramSubject",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubjectId = table.Column<int>(nullable: false),
                    EducationProgramId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationProgramSubject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationProgramSubject_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EducationProgramSubject_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationProgramSubject_EducationProgramId",
                table: "EducationProgramSubject",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationProgramSubject_SubjectId",
                table: "EducationProgramSubject",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherFiles_AspNetUsers_TeacherId",
                table: "TeacherFiles",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherFiles_AspNetUsers_TeacherId",
                table: "TeacherFiles");

            migrationBuilder.DropTable(
                name: "EducationProgramSubject");

            migrationBuilder.DropColumn(
                name: "UserDetailId",
                table: "TeacherFiles");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeacherId",
                table: "TeacherFiles",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherFiles_AspNetUsers_TeacherId",
                table: "TeacherFiles",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
