using BadMelon.Data;
using BadMelon.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class DatabaseController : Controller
    {
        private readonly BadMelonDataContext _db;
        private readonly UserManager<User> _userManager;

        public DatabaseController(BadMelonDataContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet("migrate")]
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
                await _userManager.AddPasswordAsync(await _db.Users.FirstOrDefaultAsync(), "rootpwd");
            }
            catch (Exception) { return "Cannot write to database. Contact an administrator."; }

            return "Data seeded successfuly.";
        }

        [HttpDelete]
        public async Task<string> Delete()
        {
            await _db.Database.EnsureDeletedAsync();
            return await Seed();
        }
    }
}