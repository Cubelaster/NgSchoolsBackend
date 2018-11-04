using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsDataLayer.Context
{
    public class NgSchoolsContext : IdentityDbContext<User>
    {
        public NgSchoolsContext(DbContextOptions<NgSchoolsContext> options) : base(options) { }
    }
}
