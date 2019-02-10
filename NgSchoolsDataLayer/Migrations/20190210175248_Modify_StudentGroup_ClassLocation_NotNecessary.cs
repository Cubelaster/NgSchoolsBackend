using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_StudentGroup_ClassLocation_NotNecessary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_ClassLocations_ClassLocationId",
                table: "StudentGroups");

            migrationBuilder.AlterColumn<int>(
                name: "ClassLocationId",
                table: "StudentGroups",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_ClassLocations_ClassLocationId",
                table: "StudentGroups",
                column: "ClassLocationId",
                principalTable: "ClassLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_ClassLocations_ClassLocationId",
                table: "StudentGroups");

            migrationBuilder.AlterColumn<int>(
                name: "ClassLocationId",
                table: "StudentGroups",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_ClassLocations_ClassLocationId",
                table: "StudentGroups",
                column: "ClassLocationId",
                principalTable: "ClassLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
