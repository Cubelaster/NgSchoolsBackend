using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace NgSchoolsBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            Console.WriteLine("Running NhSchools WebApi!");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
