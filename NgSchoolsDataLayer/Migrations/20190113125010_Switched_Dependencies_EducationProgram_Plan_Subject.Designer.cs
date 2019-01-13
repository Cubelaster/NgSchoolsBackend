﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NgSchoolsDataLayer.Context;

namespace NgSchoolsDataLayer.Migrations
{
    [DbContext(typeof(NgSchoolsContext))]
    [Migration("20190113125010_Switched_Dependencies_EducationProgram_Plan_Subject")]
    partial class Switched_Dependencies_EducationProgram_Plan_Subject
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.BusinessPartner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Email");

                    b.Property<string>("Mobile");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Oib");

                    b.Property<string>("Phone");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("BusinessPartners");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.ClassLocations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("ClassLocations");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.ClassType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("ClassTypes");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.EducationGroups", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("EducationGroups");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.EducationLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CognitiveSkills");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("KnowledgeBase");

                    b.Property<string>("Level")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("EducationLevels");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.EducationProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApprovalClass");

                    b.Property<string>("ComplexityLevel");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("EnrollmentConditions");

                    b.Property<string>("FinishedSchool");

                    b.Property<string>("KnoweledgeVerification");

                    b.Property<string>("Name");

                    b.Property<string>("PerformingWay");

                    b.Property<double?>("PracticalClassesDuration");

                    b.Property<string>("ProgramCompetencies");

                    b.Property<DateTime?>("ProgramDate");

                    b.Property<double?>("ProgramDuration");

                    b.Property<double?>("ProgramDurationDays");

                    b.Property<string>("ProgramDurationTextual");

                    b.Property<string>("ProgramJustifiability");

                    b.Property<string>("ShorthandName");

                    b.Property<int>("Status");

                    b.Property<double?>("TheoreticalClassesDuration");

                    b.Property<string>("UrNumber");

                    b.Property<string>("WorkingEnvironment");

                    b.HasKey("Id");

                    b.ToTable("EducationPrograms");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.ExamCommission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("ExamCommissions");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.Institution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("InstitutionClassFirstPart")
                        .IsRequired();

                    b.Property<string>("InstitutionClassSecondPart")
                        .IsRequired();

                    b.Property<string>("InstitutionCode")
                        .IsRequired();

                    b.Property<string>("InstitutionUrNumber")
                        .IsRequired();

                    b.Property<string>("Logo");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid?>("PrincipalId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("PrincipalId");

                    b.ToTable("Institution");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<int?>("EducationPogramId");

                    b.Property<int?>("EducationProgramId");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("EducationProgramId");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.PlanDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("PlanId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.ToTable("PlanDays");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.PlanDaySubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("PlanDayId");

                    b.Property<int>("Status");

                    b.Property<int>("SubjectId");

                    b.HasKey("Id");

                    b.HasIndex("PlanDayId");

                    b.HasIndex("SubjectId");

                    b.ToTable("PlanDaySubjects");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.PlanDayTheme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<double>("HoursNumber");

                    b.Property<int>("PlanDayId");

                    b.Property<int>("Status");

                    b.Property<int>("ThemeId");

                    b.HasKey("Id");

                    b.HasIndex("PlanDayId");

                    b.HasIndex("ThemeId");

                    b.ToTable("PlanDayThemes");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressCity");

                    b.Property<string>("AddressCountry");

                    b.Property<string>("AddressCounty");

                    b.Property<string>("AddressMuncipality");

                    b.Property<string>("AddressStreet");

                    b.Property<bool>("BirthCertificate");

                    b.Property<string>("CertificateNumber");

                    b.Property<string>("Citizenship");

                    b.Property<bool>("CitizenshipCertificate");

                    b.Property<string>("Cob");

                    b.Property<bool>("CompletedSchoolCertificate");

                    b.Property<string>("Couob");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<DateTime?>("Dob");

                    b.Property<bool>("DoctorCertification");

                    b.Property<bool>("DriversLicence");

                    b.Property<bool>("EducationContract");

                    b.Property<string>("Email");

                    b.Property<bool>("Employed");

                    b.Property<bool>("EmployerApplicationAttendant");

                    b.Property<string>("EmployerName");

                    b.Property<string>("EmployerOib");

                    b.Property<DateTime?>("EnrolmentDate");

                    b.Property<string>("FathersFullName");

                    b.Property<bool>("FinalExam");

                    b.Property<string>("FinishedSchool");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("Gdpr");

                    b.Property<string>("Gender");

                    b.Property<bool>("IdCard");

                    b.Property<bool>("KnowledgeTest");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Mentor");

                    b.Property<string>("Mob");

                    b.Property<string>("Mobile");

                    b.Property<string>("MothersFullName");

                    b.Property<string>("Notes");

                    b.Property<string>("Oib");

                    b.Property<string>("Photo");

                    b.Property<string>("Pob");

                    b.Property<bool>("PractiacalTrainingContract");

                    b.Property<bool>("PracticeDiary");

                    b.Property<string>("Proffesion");

                    b.Property<string>("SchoolLevel");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.StudentGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassLocationId");

                    b.Property<DateTime>("CredentialDate");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("FirstExamDate");

                    b.Property<string>("Notes");

                    b.Property<int>("ProgramId");

                    b.Property<DateTime>("SecondExamDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ClassLocationId");

                    b.HasIndex("ProgramId");

                    b.ToTable("StudentGroups");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.StudentsInGroups", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("Status");

                    b.Property<int>("StudentGroupId");

                    b.Property<int>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("StudentGroupId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentsInGroups");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EducationProgramId");

                    b.Property<string>("Literature");

                    b.Property<string>("MaterialConditions");

                    b.Property<string>("Name");

                    b.Property<string>("StaffingConditions");

                    b.Property<string>("SubjectCompetence");

                    b.Property<string>("WorkMethods");

                    b.HasKey("Id");

                    b.HasIndex("EducationProgramId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.SubjectTheme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<double>("HoursNumber");

                    b.Property<int>("Status");

                    b.Property<int>("SubjectId");

                    b.Property<int>("ThemeId");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("ThemeId");

                    b.ToTable("SubjectThemes");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("LearningOutcomes");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("UI_ThemeName");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("Status");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.UserDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("Authorization");

                    b.Property<string>("Avatar");

                    b.Property<string>("Bank");

                    b.Property<string>("Certificates");

                    b.Property<string>("City");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<string>("EmploymentPlace");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("Iban");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("MiddleName");

                    b.Property<string>("Mobile");

                    b.Property<string>("Mobile2");

                    b.Property<string>("Notes");

                    b.Property<string>("Oib");

                    b.Property<string>("Phone");

                    b.Property<bool>("PpEducation");

                    b.Property<string>("Profession");

                    b.Property<string>("Qualifications");

                    b.Property<string>("Signature");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("UserDetails");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.UserExamCommission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateModified");

                    b.Property<int>("ExamCommissionId");

                    b.Property<int>("Status");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ExamCommissionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserExamCommission");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.UserRoles", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.Institution", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.User", "Principal")
                        .WithMany()
                        .HasForeignKey("PrincipalId");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.Plan", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.EducationProgram", "EducationProgram")
                        .WithMany()
                        .HasForeignKey("EducationProgramId");
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.PlanDay", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.Plan", "Plan")
                        .WithMany("PlanDays")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.PlanDaySubject", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.PlanDay", "PlanDay")
                        .WithMany("Subjects")
                        .HasForeignKey("PlanDayId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NgSchoolsDataLayer.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.PlanDayTheme", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.PlanDay", "PlanDay")
                        .WithMany("Themes")
                        .HasForeignKey("PlanDayId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NgSchoolsDataLayer.Models.Theme", "Theme")
                        .WithMany()
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.StudentGroup", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.ClassLocations", "ClassLocation")
                        .WithMany()
                        .HasForeignKey("ClassLocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NgSchoolsDataLayer.Models.EducationProgram", "Program")
                        .WithMany()
                        .HasForeignKey("ProgramId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.StudentsInGroups", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.StudentGroup", "StudentGroup")
                        .WithMany("StudentsInGroups")
                        .HasForeignKey("StudentGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NgSchoolsDataLayer.Models.Student", "Student")
                        .WithMany("StudentsInGroups")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.Subject", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.EducationProgram", "EducationProgram")
                        .WithMany()
                        .HasForeignKey("EducationProgramId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.SubjectTheme", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.Subject", "Subject")
                        .WithMany("SubjectThemes")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NgSchoolsDataLayer.Models.Theme", "Theme")
                        .WithMany("ThemeSubjects")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.UserDetails", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.User", "User")
                        .WithOne("UserDetails")
                        .HasForeignKey("NgSchoolsDataLayer.Models.UserDetails", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.UserExamCommission", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.ExamCommission", "ExamCommission")
                        .WithMany("UserExamCommissions")
                        .HasForeignKey("ExamCommissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NgSchoolsDataLayer.Models.User", "User")
                        .WithMany("ExamCommissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NgSchoolsDataLayer.Models.UserRoles", b =>
                {
                    b.HasOne("NgSchoolsDataLayer.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NgSchoolsDataLayer.Models.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
