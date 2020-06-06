using BadMelon.API;
using BadMelon.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace BadMelon.Tests
{
    public class TestStartup : Startup
    {
        private readonly string _dbName;

        public TestStartup(IConfiguration configuration, IHostingEnvironment environment) : base(configuration, environment)
        {
            _dbName = "Test.db";
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<BadMelonDataContext>();

                if (dbContext.Database.GetDbConnection().ConnectionString.ToLower().Contains("database.windows.net"))
                {
                    throw new Exception("LIVE SETTINGS IN TESTS!");
                }

                // Initialize database
                File.Delete(_dbName);
                dbContext.Seed().Wait();
            }
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var apiAssembly = typeof(Startup).Assembly;
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                                    .PartManager.ApplicationParts.Add(new AssemblyPart(apiAssembly));

            services.AddDbContext<BadMelonDataContext>(options =>
            {
                options
                .UseLazyLoadingProxies()
                .UseSqlite($"Data Source={_dbName};");
            });
        }
    }
}