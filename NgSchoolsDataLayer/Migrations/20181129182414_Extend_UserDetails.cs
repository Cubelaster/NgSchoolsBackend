using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Extend_UserDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Authorization",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bank",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Certificates",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentPlace",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Iban",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PpEducation",
                table: "UserDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Qualifications",
                table: "UserDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Authorization",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Bank",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Certificates",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "City",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "EmploymentPlace",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Iban",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "PpEducation",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Profession",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Qualifications",
                table: "UserDetails");
        }
    }
}
