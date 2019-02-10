using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_UserDetails_CountryIdRegionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_CountryId",
                table: "UserDetails",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_RegionId",
                table: "UserDetails",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Countries_CountryId",
                table: "UserDetails",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Regions_RegionId",
                table: "UserDetails",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Countries_CountryId",
                table: "UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Regions_RegionId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_CountryId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_RegionId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "UserDetails");
        }
    }
}
