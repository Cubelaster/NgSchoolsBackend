using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NgSchoolsDataLayer.Migrations
{
    public partial class Added_Student : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Photo = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Mentor = table.Column<string>(nullable: true),
                    Oib = table.Column<string>(nullable: true),
                    Pob = table.Column<string>(nullable: true),
                    Cob = table.Column<string>(nullable: true),
                    Citizenship = table.Column<string>(nullable: true),
                    FathersFullName = table.Column<string>(nullable: true),
                    MothersFullName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    AddressStreet = table.Column<string>(nullable: true),
                    AddressCity = table.Column<string>(nullable: true),
                    AddressCounty = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Employed = table.Column<bool>(nullable: false),
                    EmployerName = table.Column<string>(nullable: true),
                    EmployerApplicationAttendant = table.Column<bool>(nullable: false),
                    EmployerOib = table.Column<string>(nullable: true),
                    EducationContract = table.Column<bool>(nullable: false),
                    FinalExam = table.Column<bool>(nullable: false),
                    CompletedSchoolCertificate = table.Column<bool>(nullable: false),
                    PractiacalTrainingContract = table.Column<bool>(nullable: false),
                    DoctorCertification = table.Column<bool>(nullable: false),
                    DriversLicence = table.Column<bool>(nullable: false),
                    KnowledgeTest = table.Column<bool>(nullable: false),
                    CitizenshipCertificate = table.Column<bool>(nullable: false),
                    BirthCertificate = table.Column<bool>(nullable: false),
                    PracticeDiary = table.Column<bool>(nullable: false),
                    EnrolmentDate = table.Column<DateTime>(nullable: true),
                    FinishedSchool = table.Column<DateTime>(nullable: true),
                    CertificateNumber = table.Column<string>(nullable: true),
                    SchoolLevel = table.Column<string>(nullable: true),
                    Gdpr = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
