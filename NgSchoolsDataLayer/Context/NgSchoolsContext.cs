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
        }
    }
}
