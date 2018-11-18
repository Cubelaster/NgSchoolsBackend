using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_InstitutionClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstitutionClassFirstPart",
                table: "Institution",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstitutionClassSecondPart",
                table: "Institution",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstitutionUrNumber",
                table: "Institution",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstitutionClassFirstPart",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "InstitutionClassSecondPart",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "InstitutionUrNumber",
                table: "Institution");
        }
    }
}
