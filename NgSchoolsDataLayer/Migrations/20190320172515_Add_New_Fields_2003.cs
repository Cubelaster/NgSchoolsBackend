using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_New_Fields_2003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkShopClasses",
                table: "Themes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CIClassesWorkShop",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegularClassesWorkShop",
                table: "EducationPrograms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkShopClasses",
                table: "Themes");

            migrationBuilder.DropColumn(
                name: "CIClassesWorkShop",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "RegularClassesWorkShop",
                table: "EducationPrograms");
        }
    }
}
