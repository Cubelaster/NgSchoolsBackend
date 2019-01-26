using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Subject_Theme_ChangeRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectThemes");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Themes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ProgramDate",
                table: "EducationPrograms",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Themes_SubjectId",
                table: "Themes",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_Subjects_SubjectId",
                table: "Themes",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Themes_Subjects_SubjectId",
                table: "Themes");

            migrationBuilder.DropIndex(
                name: "IX_Themes_SubjectId",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Themes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProgramDate",
                table: "EducationPrograms",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectThemes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    HoursNumber = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: false),
                    ThemeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectThemes_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectThemes_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectThemes_SubjectId",
                table: "SubjectThemes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectThemes_ThemeId",
                table: "SubjectThemes",
                column: "ThemeId");
        }
    }
}
