using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Linq;
using System.Reflection;

namespace NgSchoolsDataLayer.Context
{
    public class NgSchoolsContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRoles,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        private readonly IConfiguration configuration;

        public NgSchoolsContext(DbContextOptions<NgSchoolsContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        #region Db Sets

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
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ContactPerson> ContactPeople { get; set; }
        public DbSet<EducationProgramClassType> EducationProgramClassTypes { get; set; }
        public DbSet<StudentGroupClassAttendance> StudentGroupClassAttendances { get; set; }
        public DbSet<StudentClassAttendance> StudentClassAttendances { get; set; }
        public DbSet<StudentRegister> StudentRegisters { get; set; }
        public DbSet<StudentRegisterEntry> StudentRegisterEntries { get; set; }
        public DbSet<StudentRegisterEntryPrint> StudentRegisterEntryPrints { get; set; }
        public DbSet<InstitutionFile> InstitutionFiles { get; set; }
        public DbSet<TeacherFile> TeacherFiles { get; set; }
        public DbSet<EducationProgramFile> EducationProgramFiles { get; set; }

        #endregion Db Sets

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var defaultSchema = configuration.GetValue<string>("DefaultSchema");
            if (!string.IsNullOrEmpty(defaultSchema))
            {
                builder.HasDefaultSchema(defaultSchema);
            }
            base.OnModelCreating(builder);

            SetUpKeys(builder);

            SetUpGlobalFilters(builder);
        }

        private void SetUpKeys(ModelBuilder builder)
        {
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

            builder.Entity<City>()
                .HasOne(c => c.Municipality)
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
                .HasOne(s => s.AddressCountry)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
                .HasOne(s => s.AddressRegion)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentRegisterEntry>()
                .HasOne(s => s.EducationProgram)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentRegisterEntry>()
                .HasIndex(s => s.StudentsInGroupsId)
                .IsUnique(false);

            builder.Entity<EducationProgramSubject>()
                .HasOne(s => s.EducationProgram)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void SetUpGlobalFilters(ModelBuilder builder)
        {
            var dbTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t != typeof(DatabaseEntity) && typeof(DatabaseEntity).IsAssignableFrom(t))
                .ToList();

            foreach (var type in dbTypes)
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { builder });
            }
        }

        private static readonly MethodInfo SetGlobalQueryMethod = typeof(NgSchoolsContext)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetGlobalQuery));

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : DatabaseEntity
        {
            builder.Entity<T>().HasQueryFilter(e => e.Status == Enums.DatabaseEntityStatusEnum.Active);
        }
    }
}
