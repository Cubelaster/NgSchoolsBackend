using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NgSchoolsBusinessLayer.Utilities.DbUtils.Contracts;
using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Models;
using System;

namespace NgSchoolsBusinessLayer.Utilities.DbUtils
{
    public class DbInitializer : IDbInitializer
    {
        private readonly NgSchoolsContext dbContext;
        private readonly UserManager<User> userManager;

        public void Initialize(IConfigurationRoot Configuration)
        {
            throw new System.NotImplementedException();
        }

        public static void Initialize(NgSchoolsContext context)
        {
            throw new NotImplementedException();
        }
    }
}
