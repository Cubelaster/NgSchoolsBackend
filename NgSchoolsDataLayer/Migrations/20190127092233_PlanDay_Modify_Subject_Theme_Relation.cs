using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class PlanDay_Modify_Subject_Theme_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanDayThemes");

            migrationBuilder.CreateTable(
                name: "PlanDaySubjectThemes",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ThemeId = table.Column<int>(nullable: false),
                    PlanDaySubjectId = table.Column<int>(nullable: false),
                    HoursNumber = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanDaySubjectThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanDaySubjectThemes_PlanDaySubjects_PlanDaySubjectId",
                        column: x => x.PlanDaySubjectId,
                        principalTable: "PlanDaySubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanDaySubjectThemes_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanDaySubjectThemes_PlanDaySubjectId",
                table: "PlanDaySubjectThemes",
                column: "PlanDaySubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDaySubjectThemes_ThemeId",
                table: "PlanDaySubjectThemes",
                column: "ThemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanDaySubjectThemes");

            migrationBuilder.CreateTable(
                name: "PlanDayThemes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    HoursNumber = table.Column<double>(nullable: false),
                    PlanDayId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ThemeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanDayThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanDayThemes_PlanDays_PlanDayId",
                        column: x => x.PlanDayId,
                        principalTable: "PlanDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanDayThemes_Themes_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Themes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanDayThemes_PlanDayId",
                table: "PlanDayThemes",
                column: "PlanDayId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDayThemes_ThemeId",
                table: "PlanDayThemes",
                column: "ThemeId");
        }
    }
}
