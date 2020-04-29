using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadMelon.Data;
using BadMelon.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BadMelon.API.Controllers
{
    [Route("api/database/migrate")]
    public class MigrationController : Controller
    {
        private readonly BadMelonDataContext _db;
        public MigrationController(BadMelonDataContext db)
        {
            _db = db;
        }

        // GET: api/database/migrate
        [HttpGet]
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

        [Route("seed")]
        [HttpGet]
        public async Task<string> Seed()
        {
            int recipeCount;
            try
            {
                recipeCount = await _db.Recipes.CountAsync();
            }
            catch (Exception) { return "Database error - can't load entities. Contact an administrator."; }

            if (recipeCount > 0)
                return "Database not empty, cannot seed.";

            var seedRecipes = new Recipe[]
            {
                new Recipe{ ID = Guid.NewGuid(), Name = "Hot water"},
                new Recipe{ ID = Guid.NewGuid(), Name = "Cold water"}
            };

            try
            {
                await _db.Recipes.AddRangeAsync(seedRecipes);
                await _db.SaveChangesAsync();
            }
            catch (Exception) { return "Cannot write to database. Contact an administrator."; }

            return "Data seeded successfuly.";

        }

    }
}
