using Microsoft.EntityFrameworkCore;

namespace NgSchoolsDataLayer.Context
{
    public class NgSchoolsContext : DbContext
    {
        public NgSchoolsContext(DbContextOptions<NgSchoolsContext> options) : base(options) { }
    }
}
