using BadMelon.Data;
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

        public DatabaseController(BadMelonDataContext db)
        {
            _db = db;
        }

        // GET: api/database/migrate
        [HttpGet("migrate")]
        public async Task<string> Get()
        {
            try
            {
                await _db.Database.MigrateAsync();
            }
            catch (Exception e)
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
            }
            catch (Exception) { return "Cannot write to database. Contact an administrator."; }

            return "Data seeded successfuly.";
        }

        [HttpDelete]
        public async Task<string> Delete()
        {
            await _db.Database.EnsureDeletedAsync();
            await _db.Database.EnsureCreatedAsync();
            return await Seed();
        }
    }
}