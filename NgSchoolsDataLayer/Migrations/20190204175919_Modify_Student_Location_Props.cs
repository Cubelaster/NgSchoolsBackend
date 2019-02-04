using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Student_Location_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cob",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Couob",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Pob",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "CityOfBirthId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryOfBirthId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionOfBirthId",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_CityOfBirthId",
                table: "Students",
                column: "CityOfBirthId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CountryOfBirthId",
                table: "Students",
                column: "CountryOfBirthId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_RegionOfBirthId",
                table: "Students",
                column: "RegionOfBirthId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Cities_CityOfBirthId",
                table: "Students",
                column: "CityOfBirthId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Countries_CountryOfBirthId",
                table: "Students",
                column: "CountryOfBirthId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Regions_RegionOfBirthId",
                table: "Students",
                column: "RegionOfBirthId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Cities_CityOfBirthId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Countries_CountryOfBirthId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Regions_RegionOfBirthId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CityOfBirthId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CountryOfBirthId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_RegionOfBirthId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CityOfBirthId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CountryOfBirthId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RegionOfBirthId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Cob",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Couob",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pob",
                table: "Students",
                nullable: true);
        }
    }
}
