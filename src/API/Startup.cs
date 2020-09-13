using BadMelon.API.Helpers;
using BadMelon.API.Security;
using BadMelon.Data;
using BadMelon.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;
using System;

namespace BadMelon.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            services.AddAuthServices(Configuration);

            if (!Environment.IsEnvironment("Testing"))
            {
                services.AddControllers()
                    .AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()))
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            }

            var dbConnectionString = Configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                throw new Exception("Database connection could not be found.");
            }

            services.AddDbContext<BadMelonDataContext>(options =>
                    options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(dbConnectionString));

            services.AddHttpContextAccessor();

            services.AddSingleton<IEmailService, EmailService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRecipeService, RecipeService>();
            services.AddTransient<IIngredientTypeService, IngredientTypeService>();

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;

                config.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("X-version"), new QueryStringApiVersionReader("api-version"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BadMelon API", Version = "v1" });
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<BadMelonDataContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            UpdateDatabase(app);

            app.UseSerilogRequestLogging();

            app.UseBadMelonErrorHandler();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BadMelon V1");
                c.RoutePrefix = "api/swagger";
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}