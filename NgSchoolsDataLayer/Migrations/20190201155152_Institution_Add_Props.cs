using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Institution_Add_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Institution",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Institution",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Institution",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_CountryId",
                table: "Institution",
                column: "CountryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_RegionId",
                table: "Institution",
                column: "RegionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_Countries_CountryId",
                table: "Institution",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_Regions_RegionId",
                table: "Institution",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_Countries_CountryId",
                table: "Institution");

            migrationBuilder.DropForeignKey(
                name: "FK_Institution_Regions_RegionId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_CountryId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_RegionId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Institution");
        }
    }
}
