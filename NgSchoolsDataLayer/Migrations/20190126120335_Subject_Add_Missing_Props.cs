using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Subject_Add_Missing_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CollectiveConsultations",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndividualConsultations",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstConsultations",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PracticalClasses",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TheoreticalClasses",
                table: "Subjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectiveConsultations",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "IndividualConsultations",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "InstConsultations",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "PracticalClasses",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TheoreticalClasses",
                table: "Subjects");
        }
    }
}
