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

namespace BadMelon.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        public override void Configure(IApplicationBuilder app)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<BadMelonDataContext>();

                //ToDo: make this work in actual production
                if (dbContext.Database.GetDbConnection().ConnectionString.ToLower().Contains("database.windows.net"))
                {
                    throw new Exception("LIVE SETTINGS IN TESTS!");
                }

                dbContext.Database.EnsureDeletedAsync().Wait();
                base.Configure(app);

                // Initialize database
                dbContext.Seed().Wait();
            }
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            var apiAssembly = typeof(Startup).Assembly;
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                                    .PartManager.ApplicationParts.Add(new AssemblyPart(apiAssembly));
        }
    }
}