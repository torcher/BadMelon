using BadMelon.Data;
using BadMelon.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            var waterIngredientType = new IngredientType { ID = Guid.NewGuid(), Name = "Water" };
            var waterIngredients = new Ingredient[]
            {
                new Ingredient { ID = Guid.NewGuid(), Weight = 100d, IngredientTypeID = waterIngredientType.ID },
                new Ingredient { ID = Guid.NewGuid(), Weight = 100d, IngredientTypeID = waterIngredientType.ID },
            };

            var heatWater = new Step { ID = Guid.NewGuid(), Order = 1, Text = "Heat water", PrepTime = new TimeSpan(0, 0, 30) };
            var coolWater = new Step { ID = Guid.NewGuid(), Order = 1, Text = "Cool water", PrepTime = new TimeSpan(0, 0, 30) };

            seedRecipes[0].Ingredients = new List<Ingredient>();
            seedRecipes[0].Steps = new List<Step>();
            seedRecipes[0].Ingredients.Add(waterIngredients[0]);
            seedRecipes[0].Steps.Add(heatWater);

            seedRecipes[1].Ingredients = new List<Ingredient>();
            seedRecipes[1].Steps = new List<Step>();
            seedRecipes[1].Ingredients.Add(waterIngredients[1]);
            seedRecipes[1].Steps.Add(coolWater);

            try
            {
                await _db.IngredientTypes.AddAsync(waterIngredientType);
                await _db.Recipes.AddRangeAsync(seedRecipes);
                await _db.SaveChangesAsync();
            }
            catch (Exception) { return "Cannot write to database. Contact an administrator."; }

            return "Data seeded successfuly.";
        }
    }
}