using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NgSchoolsBusinessLayer.Utilities.DbUtils.Contracts;
using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsBusinessLayer.Utilities.DbUtils
{
    public class DbInitializer : IDbInitializer
    {
        private readonly NgSchoolsContext dbContext;
        private readonly UserManager<User> userManager;

        public DbInitializer(NgSchoolsContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public void Initialize(IConfigurationRoot Configuration)
        {
        }
    }
}
