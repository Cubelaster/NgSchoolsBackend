using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Institution_Modify_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Institution");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Institution",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_CityId",
                table: "Institution",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_Cities_CityId",
                table: "Institution",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_Cities_CityId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_CityId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Institution");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Institution",
                nullable: false,
                defaultValue: "");
        }
    }
}
