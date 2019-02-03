using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Tables_Missing_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "BusinessPartners");

            migrationBuilder.AddColumn<string>(
                name: "Autonomy",
                table: "EducationLevels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PsychomotorSkills",
                table: "EducationLevels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsibility",
                table: "EducationLevels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialSkills",
                table: "EducationLevels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "ClassLocations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_CityId",
                table: "BusinessPartners",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_CountryId",
                table: "BusinessPartners",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_RegionId",
                table: "BusinessPartners",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Cities_CityId",
                table: "BusinessPartners",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Countries_CountryId",
                table: "BusinessPartners",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Regions_RegionId",
                table: "BusinessPartners",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Cities_CityId",
                table: "BusinessPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Countries_CountryId",
                table: "BusinessPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Regions_RegionId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_CityId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_CountryId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_RegionId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "Autonomy",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "PsychomotorSkills",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "Responsibility",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "SocialSkills",
                table: "EducationLevels");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "BusinessPartners");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "BusinessPartners",
                nullable: true);
        }
    }
}
