using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_Plan_PlanDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanDays",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanDays_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanDaySubjects",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubjectId = table.Column<int>(nullable: false),
                    PlanDayId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanDaySubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanDaySubjects_PlanDays_PlanDayId",
                        column: x => x.PlanDayId,
                        principalTable: "PlanDays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanDaySubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanDayThemes",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ThemeId = table.Column<int>(nullable: false),
                    PlanDayId = table.Column<int>(nullable: false),
                    HoursNumber = table.Column<double>(nullable: false)
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
                name: "IX_PlanDays_PlanId",
                table: "PlanDays",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDaySubjects_PlanDayId",
                table: "PlanDaySubjects",
                column: "PlanDayId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDaySubjects_SubjectId",
                table: "PlanDaySubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDayThemes_PlanDayId",
                table: "PlanDayThemes",
                column: "PlanDayId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDayThemes_ThemeId",
                table: "PlanDayThemes",
                column: "ThemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanDaySubjects");

            migrationBuilder.DropTable(
                name: "PlanDayThemes");

            migrationBuilder.DropTable(
                name: "PlanDays");

            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
