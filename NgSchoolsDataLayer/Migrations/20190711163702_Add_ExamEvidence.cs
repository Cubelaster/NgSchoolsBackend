using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_ExamEvidence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentExamEvidences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExamDate = table.Column<DateTime>(nullable: false),
                    ExamEvidence = table.Column<string>(nullable: true),
                    StudentsInGroupsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExamEvidences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentExamEvidences_StudentsInGroups_StudentsInGroupsId",
                        column: x => x.StudentsInGroupsId,
                        principalTable: "StudentsInGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamEvidences_StudentsInGroupsId",
                table: "StudentExamEvidences",
                column: "StudentsInGroupsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentExamEvidences");
        }
    }
}
