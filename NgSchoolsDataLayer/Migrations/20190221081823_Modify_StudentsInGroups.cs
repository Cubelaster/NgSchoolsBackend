using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_StudentsInGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CompletedPractice",
                table: "StudentsInGroups",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EmployerId",
                table: "StudentsInGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentsInGroups_EmployerId",
                table: "StudentsInGroups",
                column: "EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsInGroups_BusinessPartners_EmployerId",
                table: "StudentsInGroups",
                column: "EmployerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsInGroups_BusinessPartners_EmployerId",
                table: "StudentsInGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentsInGroups_EmployerId",
                table: "StudentsInGroups");

            migrationBuilder.DropColumn(
                name: "CompletedPractice",
                table: "StudentsInGroups");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "StudentsInGroups");
        }
    }
}
