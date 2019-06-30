using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Add_GoverningCoundil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoverningCouncilId",
                table: "Institution",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GoverningCouncil",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningCouncil", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoverningCouncilMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CouncilId = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Role = table.Column<string>(nullable: true),
                    GoverningCouncilId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoverningCouncilMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoverningCouncilMember_GoverningCouncil_GoverningCouncilId",
                        column: x => x.GoverningCouncilId,
                        principalTable: "GoverningCouncil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoverningCouncilMember_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Institution_GoverningCouncilId",
                table: "Institution",
                column: "GoverningCouncilId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningCouncilMember_GoverningCouncilId",
                table: "GoverningCouncilMember",
                column: "GoverningCouncilId");

            migrationBuilder.CreateIndex(
                name: "IX_GoverningCouncilMember_UserId",
                table: "GoverningCouncilMember",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_GoverningCouncil_GoverningCouncilId",
                table: "Institution",
                column: "GoverningCouncilId",
                principalTable: "GoverningCouncil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Institution_GoverningCouncil_GoverningCouncilId",
                table: "Institution");

            migrationBuilder.DropTable(
                name: "GoverningCouncilMember");

            migrationBuilder.DropTable(
                name: "GoverningCouncil");

            migrationBuilder.DropIndex(
                name: "IX_Institution_GoverningCouncilId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "GoverningCouncilId",
                table: "Institution");
        }
    }
}
