using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Student_Modify_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressCounty",
                table: "Students",
                newName: "EmployerPhone");

            migrationBuilder.RenameColumn(
                name: "AddressCountry",
                table: "Students",
                newName: "EmployerMobile");

            migrationBuilder.RenameColumn(
                name: "AddressCity",
                table: "Students",
                newName: "EmployerEmail");

            migrationBuilder.AddColumn<int>(
                name: "AddressCityId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressCountryId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressRegionId",
                table: "Students",
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "EmployerRegionId",
                table: "Students",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_AddressCityId",
                table: "Students",
                column: "AddressCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AddressCountryId",
                table: "Students",
                column: "AddressCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AddressRegionId",
                table: "Students",
                column: "AddressRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_EmployerCityId",
                table: "Students",
                column: "EmployerCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_EmployerCountryId",
                table: "Students",
                column: "EmployerCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_EmployerRegionId",
                table: "Students",
                column: "EmployerRegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Cities_AddressCityId",
                table: "Students",
                column: "AddressCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Countries_AddressCountryId",
                table: "Students",
                column: "AddressCountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Regions_AddressRegionId",
                table: "Students",
                column: "AddressRegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Cities_AddressCityId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Countries_AddressCountryId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Regions_AddressRegionId",
                table: "Students");

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
                name: "IX_Students_AddressCityId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AddressCountryId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_AddressRegionId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_EmployerCityId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_EmployerCountryId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_EmployerRegionId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AddressCityId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AddressCountryId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AddressRegionId",
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
                name: "EmployerRegionId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "EmployerPhone",
                table: "Students",
                newName: "AddressCounty");

            migrationBuilder.RenameColumn(
                name: "EmployerMobile",
                table: "Students",
                newName: "AddressCountry");

            migrationBuilder.RenameColumn(
                name: "EmployerEmail",
                table: "Students",
                newName: "AddressCity");
        }
    }
}
