using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_UserDetails_CityId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "UserDetails");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_CityId",
                table: "UserDetails",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Cities_CityId",
                table: "UserDetails",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Cities_CityId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_CityId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "UserDetails");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserDetails",
                nullable: true);
        }
    }
}
