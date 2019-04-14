using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Student_StudentRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentRegisterEntries_StudentsInGroupsId",
                table: "StudentRegisterEntries");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegisterEntries_StudentsInGroupsId",
                table: "StudentRegisterEntries",
                column: "StudentsInGroupsId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentRegisterEntries_StudentsInGroupsId",
                table: "StudentRegisterEntries");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegisterEntries_StudentsInGroupsId",
                table: "StudentRegisterEntries",
                column: "StudentsInGroupsId");
        }
    }
}
