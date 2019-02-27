using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Utilities.DbUtils.Contracts;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using System;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Utilities.DbUtils
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(IConfigurationRoot Configuration)
        {
            throw new System.NotImplementedException();
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            CreateDefaultRoles(serviceProvider).Wait();
            CreateDefaultUsers(serviceProvider).Wait();
            FillGeoData(serviceProvider).Wait();
        }

        private static async Task CreateDefaultRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { "Admin", "Super Admin", "Ravnatelj", "Voditelj obrazovanja", "Nastavnik", "Korisnik" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new Role { Name = roleName });
                }
            }
        }

        private static async Task CreateDefaultUsers(IServiceProvider serviceProvider)
        {
            //Here you could create a super user who will maintain the web app
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var Configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var defaultUsers = Configuration.GetSection("DefaultUsers").GetChildren();
            foreach (var user in defaultUsers)
            {
                var appUser = new User
                {
                    UserName = user[nameof(User.UserName)],
                    Email = user[nameof(User.Email)],
                    EmailConfirmed = true,
                    DateCreated = DateTime.UtcNow,
                    Status = DatabaseEntityStatusEnum.Active,
                    UserDetails = new UserDetails
                    {
                        FirstName = user[nameof(UserDetails.FirstName)],
                        LastName = user[nameof(UserDetails.LastName)]
                    }
                };

                if (await userManager.FindByNameAsync(appUser.UserName) == null)
                {
                    await userManager.CreateAsync(appUser, user.GetValue<string>("Password"));
                    await userManager.AddToRoleAsync(await userManager.FindByNameAsync(appUser.UserName), "Super Admin");
                }
            }
        }

        private static async Task FillGeoData(IServiceProvider serviceProvider)
        {
            var locationService = serviceProvider.GetService<ILocationService>();
            await locationService.SeedLocationData();
        }
    }
}
