using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Modify_UserDetails_Switch_To_UploadedFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "UserDetails");

            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_AvatarId",
                table: "UserDetails",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_SignatureId",
                table: "UserDetails",
                column: "SignatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_UploadedFiles_AvatarId",
                table: "UserDetails",
                column: "AvatarId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_UploadedFiles_SignatureId",
                table: "UserDetails",
                column: "SignatureId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_UploadedFiles_AvatarId",
                table: "UserDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_UploadedFiles_SignatureId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_AvatarId",
                table: "UserDetails");

            migrationBuilder.DropIndex(
                name: "IX_UserDetails_SignatureId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "UserDetails");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "UserDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "UserDetails",
                nullable: true);
        }
    }
}
