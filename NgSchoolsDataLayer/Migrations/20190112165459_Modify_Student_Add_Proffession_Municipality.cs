using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_Student_Add_Proffession_Municipality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressMuncipality",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Proffesion",
                table: "Students",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressMuncipality",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Proffesion",
                table: "Students");
        }
    }
}
