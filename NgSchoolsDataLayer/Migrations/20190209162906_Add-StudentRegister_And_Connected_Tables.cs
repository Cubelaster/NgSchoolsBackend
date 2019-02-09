using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class AddStudentRegister_And_Connected_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentRegisters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BookNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegisters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentRegisterEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StudentRegisterNumber = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    StudentsInGroupsId = table.Column<int>(nullable: false),
                    StudentRegisterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegisterEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRegisterEntries_StudentRegisters_StudentRegisterId",
                        column: x => x.StudentRegisterId,
                        principalTable: "StudentRegisters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentRegisterEntries_StudentsInGroups_StudentsInGroupsId",
                        column: x => x.StudentsInGroupsId,
                        principalTable: "StudentsInGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentRegisterEntryPrints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PrintDate = table.Column<DateTime>(nullable: false),
                    StudentRegisterEntryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegisterEntryPrints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRegisterEntryPrints_StudentRegisterEntries_StudentRegisterEntryId",
                        column: x => x.StudentRegisterEntryId,
                        principalTable: "StudentRegisterEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegisterEntries_StudentRegisterId",
                table: "StudentRegisterEntries",
                column: "StudentRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegisterEntries_StudentsInGroupsId",
                table: "StudentRegisterEntries",
                column: "StudentsInGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegisterEntryPrints_StudentRegisterEntryId",
                table: "StudentRegisterEntryPrints",
                column: "StudentRegisterEntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentRegisterEntryPrints");

            migrationBuilder.DropTable(
                name: "StudentRegisterEntries");

            migrationBuilder.DropTable(
                name: "StudentRegisters");
        }
    }
}
