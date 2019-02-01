using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Various_Modify_Props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessPartnerContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactPerson",
                table: "ContactPerson");

            migrationBuilder.DropColumn(
                name: "PracticalClassesDuration",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "TheoreticalClassesDuration",
                table: "EducationPrograms");

            migrationBuilder.RenameTable(
                name: "ContactPerson",
                newName: "ContactPeople");

            migrationBuilder.RenameColumn(
                name: "FinishedSchool",
                table: "EducationPrograms",
                newName: "RegularClassesTeoretical");

            migrationBuilder.AddColumn<string>(
                name: "AgencyApprovalClass",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AgencyProgramDate",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgencyUrNumber",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CIClassesGroup",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CIClassesIndividual",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CIClassesPractical",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegularClassesPractical",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessPartnerId",
                table: "ContactPeople",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ContactPeople",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "ContactPeople",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ContactPeople",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactPeople",
                table: "ContactPeople",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EducationProgramClassTypes",
                columns: table => new
                {
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EducationProgramId = table.Column<int>(nullable: false),
                    ClassTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationProgramClassTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationProgramClassTypes_ClassTypes_ClassTypeId",
                        column: x => x.ClassTypeId,
                        principalTable: "ClassTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationProgramClassTypes_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactPeople_BusinessPartnerId",
                table: "ContactPeople",
                column: "BusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationProgramClassTypes_ClassTypeId",
                table: "EducationProgramClassTypes",
                column: "ClassTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationProgramClassTypes_EducationProgramId",
                table: "EducationProgramClassTypes",
                column: "EducationProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactPeople_BusinessPartners_BusinessPartnerId",
                table: "ContactPeople",
                column: "BusinessPartnerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactPeople_BusinessPartners_BusinessPartnerId",
                table: "ContactPeople");

            migrationBuilder.DropTable(
                name: "EducationProgramClassTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactPeople",
                table: "ContactPeople");

            migrationBuilder.DropIndex(
                name: "IX_ContactPeople_BusinessPartnerId",
                table: "ContactPeople");

            migrationBuilder.DropColumn(
                name: "AgencyApprovalClass",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "AgencyProgramDate",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "AgencyUrNumber",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "CIClassesGroup",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "CIClassesIndividual",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "CIClassesPractical",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "RegularClassesPractical",
                table: "EducationPrograms");

            migrationBuilder.DropColumn(
                name: "BusinessPartnerId",
                table: "ContactPeople");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ContactPeople");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "ContactPeople");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ContactPeople");

            migrationBuilder.RenameTable(
                name: "ContactPeople",
                newName: "ContactPerson");

            migrationBuilder.RenameColumn(
                name: "RegularClassesTeoretical",
                table: "EducationPrograms",
                newName: "FinishedSchool");

            migrationBuilder.AddColumn<double>(
                name: "PracticalClassesDuration",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TheoreticalClassesDuration",
                table: "EducationPrograms",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactPerson",
                table: "ContactPerson",
                column: "Id");

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
    }
}
