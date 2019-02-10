using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NgSchoolsDataLayer.Models;
using System;

namespace NgSchoolsDataLayer.Context
{
    public class NgSchoolsContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRoles,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public NgSchoolsContext(DbContextOptions<NgSchoolsContext> options) : base(options) { }

        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<ClassLocations> ClassLocations { get; set; }
        public DbSet<ClassType> ClassTypes { get; set; }
        public DbSet<EducationGroups> EducationGroups { get; set; }
        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<ExamCommission> ExamCommissions { get; set; }
        public DbSet<Institution> Institution { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<EducationProgram> EducationPrograms { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanDay> PlanDays { get; set; }
        public DbSet<PlanDaySubject> PlanDaySubjects { get; set; }
        public DbSet<PlanDaySubjectTheme> PlanDaySubjectThemes { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<StudentFiles> StudentFiles { get; set; }
        public DbSet<Diary> Diaries { get; set; }
        public DbSet<DiaryStudentGroup> DiaryStudentGroups { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ContactPerson> ContactPeople { get; set; }
        public DbSet<EducationProgramClassType> EducationProgramClassTypes { get; set; }
        public DbSet<StudentGroupClassAttendance> StudentGroupClassAttendances { get; set; }
        public DbSet<StudentClassAttendance> StudentClassAttendances { get; set; }
        public DbSet<StudentRegister> StudentRegisters { get; set; }
        public DbSet<StudentRegisterEntry> StudentRegisterEntries { get; set; }
        public DbSet<StudentRegisterEntryPrint> StudentRegisterEntryPrints { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRoles>(userRole =>
            {
                builder.Entity<User>()
                    .HasIndex(u => u.Email)
                    .IsUnique();

                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.Roles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<User>()
                .HasOne(u => u.UserDetails)
                .WithOne(ud => ud.User)
                .HasForeignKey<UserDetails>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Plan>()
                .HasMany(p => p.PlanDays)
                .WithOne(pd => pd.Plan);

            builder.Entity<PlanDay>()
                .HasMany(pd => pd.Subjects)
                .WithOne(pds => pds.PlanDay);

            builder.Entity<PlanDaySubject>()
                .HasOne(pds => pds.PlanDay)
                .WithMany(pd => pd.Subjects);

            builder.Entity<PlanDaySubject>()
                .HasOne(pds => pds.Subject)
                .WithMany(s => s.PlanDaySubjects)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Subject>()
                .HasMany(s => s.PlanDaySubjects)
                .WithOne(pds => pds.Subject)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PlanDaySubject>()
                .HasMany(pds => pds.PlanDaySubjectThemes)
                .WithOne(pdst => pdst.PlanDaySubject)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Theme>()
                .HasMany(t => t.PlanDaySubjectThemes)
                .WithOne(pdst => pdst.Theme)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<City>()
                .HasOne(c => c.Region)
                .WithMany(r => r.Cities)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Institution>()
                .HasOne(i => i.Region)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Institution>()
                .HasOne(i => i.Country)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClassLocations>()
                .HasOne(cl => cl.Country)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClassLocations>()
                .HasOne(cl => cl.City)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClassLocations>()
                .HasOne(cl => cl.Region)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                .HasOne(s => s.AddressCity)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                .HasOne(s => s.EmployerCity)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                .HasOne(s => s.AddressCountry)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                .HasOne(s => s.EmployerCountry)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                .HasOne(s => s.AddressRegion)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                .HasOne(s => s.EmployerRegion)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentRegisterEntry>()
                .HasOne(s => s.EducationProgram)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
