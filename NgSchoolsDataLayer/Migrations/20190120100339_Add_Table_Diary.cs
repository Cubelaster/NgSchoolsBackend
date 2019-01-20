using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_Table_Diary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiaryId",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Diaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    EducationalGroupMark = table.Column<string>(nullable: true),
                    SchoolYear = table.Column<string>(nullable: true),
                    EducationalPeriod = table.Column<DateTime>(nullable: false),
                    Class = table.Column<string>(nullable: true),
                    EducationProgramType = table.Column<string>(nullable: true),
                    PerformingWay = table.Column<string>(nullable: true),
                    OpenDate = table.Column<DateTime>(nullable: true),
                    CloseDate = table.Column<DateTime>(nullable: true),
                    EducationGroupId = table.Column<int>(nullable: true),
                    EducationLeaderId = table.Column<Guid>(nullable: true),
                    ClassOfficerId = table.Column<Guid>(nullable: true),
                    ClassTypeId = table.Column<int>(nullable: true),
                    TeachingPlaceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diaries_AspNetUsers_ClassOfficerId",
                        column: x => x.ClassOfficerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Diaries_ClassTypes_ClassTypeId",
                        column: x => x.ClassTypeId,
                        principalTable: "ClassTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Diaries_EducationGroups_EducationGroupId",
                        column: x => x.EducationGroupId,
                        principalTable: "EducationGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Diaries_AspNetUsers_EducationLeaderId",
                        column: x => x.EducationLeaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Diaries_ClassLocations_TeachingPlaceId",
                        column: x => x.TeachingPlaceId,
                        principalTable: "ClassLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_DiaryId",
                table: "StudentGroups",
                column: "DiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Diaries_ClassOfficerId",
                table: "Diaries",
                column: "ClassOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_Diaries_ClassTypeId",
                table: "Diaries",
                column: "ClassTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Diaries_EducationGroupId",
                table: "Diaries",
                column: "EducationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Diaries_EducationLeaderId",
                table: "Diaries",
                column: "EducationLeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Diaries_TeachingPlaceId",
                table: "Diaries",
                column: "TeachingPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_Diaries_DiaryId",
                table: "StudentGroups",
                column: "DiaryId",
                principalTable: "Diaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_Diaries_DiaryId",
                table: "StudentGroups");

            migrationBuilder.DropTable(
                name: "Diaries");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_DiaryId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "DiaryId",
                table: "StudentGroups");
        }
    }
}
