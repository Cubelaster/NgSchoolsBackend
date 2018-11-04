using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Services.Implementations;
using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Models;
using System;
using System.IO;

namespace NgSchoolsBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NgSchoolsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NgSchoolsConnection"),
                opts => opts.MigrationsAssembly("NgSchoolsDataLayer")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<NgSchoolsContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1);

                options.LoginPath = "/api/Auth/Login";
                options.AccessDeniedPath = "/api/Auth/AccessDenied";
                options.SlidingExpiration = false;
            });

            // Part of Remember me
            //services.AddDataProtection()
            //    .SetApplicationName($"{Environment.ApplicationName}-{Environment.EnvironmentName}")
            //    .PersistKeysToFileSystem(new DirectoryInfo($@"{Environment.ContentRootPath}\keys"));

            ConfigureServicesDI(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void ConfigureServicesDI(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
