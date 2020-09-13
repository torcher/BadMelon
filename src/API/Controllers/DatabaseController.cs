using BadMelon.Data;
using BadMelon.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.API.Controllers
{
    [Route("api/[controller]")]
    public class DatabaseController : Controller
    {
        private readonly BadMelonDataContext _db;

        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _host;

        public DatabaseController(BadMelonDataContext db, IHostingEnvironment host, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
            _host = host;
        }

        [HttpGet("migrate")]
        [Authorize]
        public async Task<string> Get()
        {
            try
            {
                await _db.Database.MigrateAsync();
            }
            catch (Exception)
            {
                return "Database migration failed. Contact an administrator.";
            }

            try
            {
                var recipe = await _db.Recipes.Take(1).ToArrayAsync();
                return "Database migrated successfully.";
            }
            catch (ArgumentOutOfRangeException)
            {
                return "Database migrated, but no seed data found.";
            }
        }

        [HttpGet("seed")]
        [Authorize]
        public async Task<string> Seed()
        {
            await _db.Database.EnsureCreatedAsync();
            int recipeCount;
            try
            {
                recipeCount = await _db.Recipes.CountAsync();
            }
            catch (Exception) { return "Database error - can't load entities. Contact an administrator."; }

            if (recipeCount > 0)
                return "Database not empty, cannot seed.";

            try
            {
                await _db.Seed();
                var rootpasswordResult = await _userManager.AddPasswordAsync(await _db.Users.FirstOrDefaultAsync(), "rootpassword");
            }
            catch (Exception)
            {
                return "Cannot write to database. Contact an administrator.";
            }

            return "Data seeded successfuly.";
        }

        [HttpDelete]
        public async Task<string> Delete()
        {
            if (_host.IsDevelopment() || _host.IsEnvironment("Testing"))
            {
                await _db.Database.EnsureDeletedAsync();
                await _db.Database.MigrateAsync();
                return await Seed();
            }
            return ErrorResponse();
        }

        private string ErrorResponse()
        {
            HttpContext.Response.StatusCode = 404;
            return string.Empty;
        }
    }
}