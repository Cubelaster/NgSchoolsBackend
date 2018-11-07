﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Services.Implementations;
using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.Contracts;
using NgSchoolsDataLayer.Repository.Implementations;
using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NgSchoolsBusinessLayer.Security.Jwt;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Security.Jwt.Implementations;

namespace NgSchoolsBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private SymmetricSecurityKey signingKey;
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // TODO: Add right Origin
                options.AddPolicy("DevelopmentPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyOrigin()
                );
            });

            services.AddDbContext<NgSchoolsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NgSchoolsConnection"),
                opts => opts.MigrationsAssembly("NgSchoolsDataLayer")));

            ConfigureServicesDI(services);

            ConfigureJWT(services);

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<NgSchoolsContext>()
                .AddRoles<IdentityRole<Guid>>()
                .AddDefaultTokenProviders();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var secretKey = jwtAppSettingOptions.GetValue<string>("SecretKey");
            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            ConfigureAuthorization(services);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config =>
            {
                config.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        UserDto user = (await userService.GetUserByName(context.Principal.Identity.Name)).GetData();
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        context.Success();
                    }
                };
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAutoMapper();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevelopmentPolicy");
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();
            app.UseMvc();

            CreateDefaultRoles(serviceProvider).Wait();
            CreateDefaultUsers(serviceProvider).Wait();
        }

        private void ConfigureServicesDI(IServiceCollection services)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
        }

        private void ConfigureJWT(IServiceCollection services)
        {
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var secretKey = jwtAppSettingOptions.GetValue<string>("SecretKey");
            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Admin")
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
            });
        }

        private async Task CreateDefaultRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            string[] roleNames = { "Admin", "Professor", "Student" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }
        }

        private async Task CreateDefaultUsers(IServiceProvider serviceProvider)
        {
            //Here you could create a super user who will maintain the web app
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

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
                    FirstName = user[nameof(User.FirstName)],
                    LastName = user[nameof(User.LastName)]
                };

                if (await userManager.FindByNameAsync(appUser.UserName) == null)
                {
                    await userManager.CreateAsync(appUser, user.GetValue<string>("Password"));
                    await userManager.AddToRoleAsync(await userManager.FindByNameAsync(appUser.UserName), "Admin");
                }
            }
        }
    }
}
