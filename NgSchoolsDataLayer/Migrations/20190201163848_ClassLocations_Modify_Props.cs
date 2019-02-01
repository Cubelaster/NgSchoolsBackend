using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class ClassLocations_Modify_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "ClassLocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "ClassLocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ClassLocations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "ClassLocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "ClassLocations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassLocations_CityId",
                table: "ClassLocations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLocations_CountryId",
                table: "ClassLocations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLocations_RegionId",
                table: "ClassLocations",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassLocations_Cities_CityId",
                table: "ClassLocations",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassLocations_Countries_CountryId",
                table: "ClassLocations",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassLocations_Regions_RegionId",
                table: "ClassLocations",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassLocations_Cities_CityId",
                table: "ClassLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassLocations_Countries_CountryId",
                table: "ClassLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassLocations_Regions_RegionId",
                table: "ClassLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClassLocations_CityId",
                table: "ClassLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClassLocations_CountryId",
                table: "ClassLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClassLocations_RegionId",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "ClassLocations");
        }
    }
}
