using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Propagate_Municipality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Institution_RegionId",
                table: "Institution");

            migrationBuilder.AddColumn<int>(
                name: "MunicipalityId",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressMunicipalityId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MunicipalityOfBirthId",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DirectorId",
                table: "StudentGroups",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Institution",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "MunicipalityId",
                table: "Institution",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "ContactPeople",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "ClassLocations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "MunicipalityId",
                table: "ClassLocations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MunicipalityId",
                table: "BusinessPartners",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_MunicipalityId",
                table: "UserDetails",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AddressMunicipalityId",
                table: "Students",
                column: "AddressMunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_MunicipalityOfBirthId",
                table: "Students",
                column: "MunicipalityOfBirthId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_DirectorId",
                table: "StudentGroups",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_MunicipalityId",
                table: "Institution",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_RegionId",
                table: "Institution",
                column: "RegionId",
                unique: true,
                filter: "[RegionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClassLocations_MunicipalityId",
                table: "ClassLocations",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_MunicipalityId",
                table: "BusinessPartners",
                column: "MunicipalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_Municipalities_MunicipalityId",
                table: "BusinessPartners",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassLocations_Municipalities_MunicipalityId",
                table: "ClassLocations",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Institution_Municipalities_MunicipalityId",
                table: "Institution",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_AspNetUsers_DirectorId",
                table: "StudentGroups",
                column: "DirectorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Municipalities_AddressMunicipalityId",
                table: "Students",
                column: "AddressMunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Municipalities_MunicipalityOfBirthId",
                table: "Students",
                column: "MunicipalityOfBirthId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Municipalities_MunicipalityId",
                table: "UserDetails",
                column: "MunicipalityId",
                principalTable: "Municipalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_Municipalities_MunicipalityId",
                table: "BusinessPartners");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassLocations_Municipalities_MunicipalityId",
                table: "ClassLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Institution_Municipalities_MunicipalityId",
                table: "Institution");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_AspNetUsers_DirectorId",
                table: "StudentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Municipalities_AddressMunicipalityId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Municipalities_MunicipalityOfBirthId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Municipalities_MunicipalityId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_MunicipalityId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_Students_AddressMunicipalityId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_MunicipalityOfBirthId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_DirectorId",
                table: "StudentGroups");

            migrationBuilder.DropIndex(
                name: "IX_Institution_MunicipalityId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_Institution_RegionId",
                table: "Institution");

            migrationBuilder.DropIndex(
                name: "IX_ClassLocations_MunicipalityId",
                table: "ClassLocations");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_MunicipalityId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "MunicipalityId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "AddressMunicipalityId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MunicipalityOfBirthId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DirectorId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "MunicipalityId",
                table: "Institution");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "ContactPeople");

            migrationBuilder.DropColumn(
                name: "MunicipalityId",
                table: "ClassLocations");

            migrationBuilder.DropColumn(
                name: "MunicipalityId",
                table: "BusinessPartners");

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "Institution",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegionId",
                table: "ClassLocations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Institution_RegionId",
                table: "Institution",
                column: "RegionId",
                unique: true);
        }
    }
}
