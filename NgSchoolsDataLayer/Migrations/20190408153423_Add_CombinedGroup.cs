using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_CombinedGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CombinedGroupId",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CombinedGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombinedGroup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_CombinedGroupId",
                table: "StudentGroups",
                column: "CombinedGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_CombinedGroup_CombinedGroupId",
                table: "StudentGroups",
                column: "CombinedGroupId",
                principalTable: "CombinedGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_CombinedGroup_CombinedGroupId",
                table: "StudentGroups");

            migrationBuilder.DropTable(
                name: "CombinedGroup");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_CombinedGroupId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "CombinedGroupId",
                table: "StudentGroups");
        }
    }
}
