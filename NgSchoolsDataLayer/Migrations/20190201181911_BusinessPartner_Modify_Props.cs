using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class BusinessPartner_Modify_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactPerson",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPerson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessPartnerContacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BusinessPartnerId = table.Column<int>(nullable: false),
                    ContactPersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartnerContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerContacts_BusinessPartners_BusinessPartnerId",
                        column: x => x.BusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessPartnerContacts_ContactPerson_ContactPersonId",
                        column: x => x.ContactPersonId,
                        principalTable: "ContactPerson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerContacts_BusinessPartnerId",
                table: "BusinessPartnerContacts",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartnerContacts_ContactPersonId",
                table: "BusinessPartnerContacts",
                column: "ContactPersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessPartnerContacts");

            migrationBuilder.DropTable(
                name: "ContactPerson");
        }
    }
}
