using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_StudentGroup_Add_Location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassLocationId",
                table: "StudentGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_ClassLocationId",
                table: "StudentGroups",
                column: "ClassLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_ClassLocations_ClassLocationId",
                table: "StudentGroups",
                column: "ClassLocationId",
                principalTable: "ClassLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_ClassLocations_ClassLocationId",
                table: "StudentGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_ClassLocationId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "ClassLocationId",
                table: "StudentGroups");
        }
    }
}
