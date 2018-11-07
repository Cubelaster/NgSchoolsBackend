using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NgSchoolsDataLayer.Models;
using System;

namespace NgSchoolsDataLayer.Context
{
    public class NgSchoolsContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public NgSchoolsContext(DbContextOptions<NgSchoolsContext> options) : base(options) { }
    }
}
