using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Institution_Location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Institution_CountryId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_RegionId",
                table: "Institution");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_CountryId",
                table: "Institution",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_RegionId",
                table: "Institution",
                column: "RegionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Institution_CountryId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_RegionId",
                table: "Institution");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_CountryId",
                table: "Institution",
                column: "CountryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_RegionId",
                table: "Institution",
                column: "RegionId",
                unique: true,
                filter: "[RegionId] IS NOT NULL");
        }
    }
}
