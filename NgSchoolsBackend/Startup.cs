using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NgSchoolsBusinessLayer.Security.Jwt;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Security.Jwt.Implementations;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Services.Implementations;
using NgSchoolsBusinessLayer.Services.Implementations.Common;
using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
            services.AddMemoryCache();

            services.AddDbContext<NgSchoolsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NgSchoolsConnection"),
                opts => opts.MigrationsAssembly("NgSchoolsDataLayer")));

            ConfigureServicesDI(services);

            ConfigureJWT(services);

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<NgSchoolsContext>()
                .AddRoles<IdentityRole<Guid>>()
                .AddDefaultTokenProviders();

            services.AddLogging(options =>
            {
                var eventLogSettings = new EventLogSettings
                {
                    LogName = "NgSchools",
                    SourceName = "NgSchools",

                };
                options.AddEventLog(eventLogSettings);
            });

            services.AddAutoMapper();

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    var resolver = options.SerializerSettings.ContractResolver;
                    if (resolver != null)
                    {
                        var res = resolver as DefaultContractResolver;
                        res.NamingStrategy = null;  // <<!-- this removes the camelcasing
                    }
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            ConfigureFileServer(app);

            app.UseCors(options =>
            {
                options.AllowCredentials();
                options.AllowAnyHeader();
                options.WithMethods(new string[] { "POST", "GET", "OPTIONS" });
                options.WithOrigins(Configuration.GetValue<string>("CorsOrigin"));
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            CreateDefaultRoles(serviceProvider).Wait();
            CreateDefaultUsers(serviceProvider).Wait();
        }

        private void ConfigureFileServer(IApplicationBuilder app)
        {
            string fileServerFolder = Configuration.GetValue<string>("UploadDestination");
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), fileServerFolder.Replace("/", ""));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(directoryPath),
                RequestPath = fileServerFolder,
                EnableDirectoryBrowsing = false
            });
        }

        private void ConfigureServicesDI(IServiceCollection services)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<ILoggerService, LoggerService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserDetailsService, UserDetailsService>();
            services.AddScoped<IInstitutionService, InstitutionService>();
            services.AddScoped<IClassTypeService, ClassTypeService>();
            services.AddScoped<IClassLocationsService, ClassLocationsService>();
            services.AddScoped<IEducationGroupService, EducationGroupService>();
            services.AddScoped<IEducationLevelService, EducationLevelService>();
            services.AddScoped<IExamCommissionService, ExamCommissionService>();
            services.AddScoped<IBusinessPartnerService, BusinessPartnerService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IEducationProgramService, EducationProgramService>();
            services.AddScoped<IStudentGroupService, StudentGroupService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<ISubjectService, SubjectService>();
        }

        private void ConfigureJWT(IServiceCollection services)
        {
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var secretKey = jwtAppSettingOptions.GetValue<string>("SecretKey");
            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                    IssuerSigningKey = signingKey
                };
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
            });

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });
        }

        private async Task CreateDefaultRoles(IServiceProvider serviceProvider)
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
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new Role { Name = roleName });
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
    }
}
