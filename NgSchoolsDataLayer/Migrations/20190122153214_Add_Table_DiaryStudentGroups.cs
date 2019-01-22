using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_Table_DiaryStudentGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_Diaries_DiaryId",
                table: "StudentGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_DiaryId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "DiaryId",
                table: "StudentGroups");

            migrationBuilder.CreateTable(
                name: "DiaryStudentGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DiaryId = table.Column<int>(nullable: false),
                    StudentGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryStudentGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaryStudentGroups_Diaries_DiaryId",
                        column: x => x.DiaryId,
                        principalTable: "Diaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiaryStudentGroups_StudentGroups_StudentGroupId",
                        column: x => x.StudentGroupId,
                        principalTable: "StudentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaryStudentGroups_DiaryId",
                table: "DiaryStudentGroups",
                column: "DiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_DiaryStudentGroups_StudentGroupId",
                table: "DiaryStudentGroups",
                column: "StudentGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaryStudentGroups");

            migrationBuilder.AddColumn<int>(
                name: "DiaryId",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_DiaryId",
                table: "StudentGroups",
                column: "DiaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_Diaries_DiaryId",
                table: "StudentGroups",
                column: "DiaryId",
                principalTable: "Diaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
