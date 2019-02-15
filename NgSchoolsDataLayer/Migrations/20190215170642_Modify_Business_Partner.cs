using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Business_Partner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Cities_EmployerCityId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Countries_EmployerCountryId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Regions_EmployerRegionId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_EmployerCityId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_EmployerCountryId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmployerAddress",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmployerCityId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmployerCountryId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmployerEmail",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmployerMobile",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "EmployerPhone",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "EmployerRegionId",
                table: "Students",
                newName: "EmployerId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_EmployerRegionId",
                table: "Students",
                newName: "IX_Students_EmployerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsBusinessPartner",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmployer",
                table: "BusinessPartners",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_BusinessPartners_EmployerId",
                table: "Students",
                column: "EmployerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_BusinessPartners_EmployerId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsBusinessPartner",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "IsEmployer",
                table: "BusinessPartners");

            migrationBuilder.RenameColumn(
                name: "EmployerId",
                table: "Students",
                newName: "EmployerRegionId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_EmployerId",
                table: "Students",
                newName: "IX_Students_EmployerRegionId");

            migrationBuilder.AddColumn<string>(
                name: "EmployerAddress",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployerCityId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployerCountryId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerEmail",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerMobile",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployerPhone",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_EmployerCityId",
                table: "Students",
                column: "EmployerCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_EmployerCountryId",
                table: "Students",
                column: "EmployerCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Cities_EmployerCityId",
                table: "Students",
                column: "EmployerCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Countries_EmployerCountryId",
                table: "Students",
                column: "EmployerCountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Regions_EmployerRegionId",
                table: "Students",
                column: "EmployerRegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
