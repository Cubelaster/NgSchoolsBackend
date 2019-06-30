using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_GoverningCouncilMembers_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoverningCouncilMember_GoverningCouncil_GoverningCouncilId",
                table: "GoverningCouncilMember");

            migrationBuilder.DropColumn(
                name: "CouncilId",
                table: "GoverningCouncilMember");

            migrationBuilder.AlterColumn<int>(
                name: "GoverningCouncilId",
                table: "GoverningCouncilMember",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GoverningCouncilMember_GoverningCouncil_GoverningCouncilId",
                table: "GoverningCouncilMember",
                column: "GoverningCouncilId",
                principalTable: "GoverningCouncil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoverningCouncilMember_GoverningCouncil_GoverningCouncilId",
                table: "GoverningCouncilMember");

            migrationBuilder.AlterColumn<int>(
                name: "GoverningCouncilId",
                table: "GoverningCouncilMember",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CouncilId",
                table: "GoverningCouncilMember",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_GoverningCouncilMember_GoverningCouncil_GoverningCouncilId",
                table: "GoverningCouncilMember",
                column: "GoverningCouncilId",
                principalTable: "GoverningCouncil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
