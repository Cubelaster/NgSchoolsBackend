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
    }
}
