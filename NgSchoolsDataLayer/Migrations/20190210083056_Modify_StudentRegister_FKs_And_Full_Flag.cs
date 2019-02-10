using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_StudentRegister_FKs_And_Full_Flag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRegisterEntries_StudentRegisters_StudentRegisterId",
                table: "StudentRegisterEntries");

            migrationBuilder.AddColumn<bool>(
                name: "Full",
                table: "StudentRegisters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "StudentRegisterId",
                table: "StudentRegisterEntries",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRegisterEntries_StudentRegisters_StudentRegisterId",
                table: "StudentRegisterEntries",
                column: "StudentRegisterId",
                principalTable: "StudentRegisters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRegisterEntries_StudentRegisters_StudentRegisterId",
                table: "StudentRegisterEntries");

            migrationBuilder.DropColumn(
                name: "Full",
                table: "StudentRegisters");

            migrationBuilder.AlterColumn<int>(
                name: "StudentRegisterId",
                table: "StudentRegisterEntries",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRegisterEntries_StudentRegisters_StudentRegisterId",
                table: "StudentRegisterEntries",
                column: "StudentRegisterId",
                principalTable: "StudentRegisters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
