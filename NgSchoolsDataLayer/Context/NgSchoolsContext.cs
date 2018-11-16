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

        public DbSet<Institution> Institution { get; set; }

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
        }
    }
}
