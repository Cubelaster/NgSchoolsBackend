using Microsoft.EntityFrameworkCore;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsDataLayer.Context
{
    public class NgSchoolsContext : DbContext
    {
        public NgSchoolsContext(DbContextOptions<NgSchoolsContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
