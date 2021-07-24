using BadMelon.Data;
using BadMelon.Data.Entities;
using BadMelon.Data.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BadMelon.API.Security
{
    public static class ServiceRegistration
    {
        public static void AddAuthServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<BadMelonDataContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;// true;
                options.Password.RequireLowercase = false;//true;
                options.Password.RequireNonAlphanumeric = false;//true;
                options.Password.RequireUppercase = false;//true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_+={}[]\\|;:,.<>?/";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });
        }
    }
}