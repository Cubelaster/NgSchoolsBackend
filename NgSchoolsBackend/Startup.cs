using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NgSchoolsBusinessLayer.Security.Jwt;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Security.Jwt.Implementations;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Services.Implementations;
using NgSchoolsBusinessLayer.Services.Implementations.Common;
using NgSchoolsBusinessLayer.Utilities.Automapper.Profiles;
using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using NgSchoolsWebApi.Utilities;
using System;
using System.IO;
using System.Text;

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

            services.AddHttpClient();

            services.AddDbContext<NgSchoolsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NgSchoolsConnection"),
                opts => opts.MigrationsAssembly("NgSchoolsDataLayer")));

            //LoadDinkDll(services);

            ConfigureServicesDI(services);

            ConfigureJWT(services);

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<NgSchoolsContext>()
                .AddRoles<IdentityRole<Guid>>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(UserMapper));

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatString = "dd.MM.yyyy.";
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
                options.WithOrigins(Configuration.GetSection("CorsOrigin").Get<string[]>());
            });

            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseAuthentication();
            app.UseMvc();
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

        private void LoadDinkDll(IServiceCollection services)
        {
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
        }

        private void ConfigureServicesDI(IServiceCollection services)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

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
            services.AddScoped<IThemeService, ThemeService>();
            services.AddScoped<IDiaryService, DiaryService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IStudentRegisterService, StudentRegisterService>();
            services.AddScoped<IPdfGeneratorService, PdfGeneratorService>();
        }

        private void ConfigureJWT(IServiceCollection services)
        {
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var secretKey = jwtAppSettingOptions.GetValue<string>("SecretKey");
            signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
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
    }
}
