﻿using BadMelon.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BadMelon.Data
{
    public class BadMelonDataContext : DbContext
    {
        public BadMelonDataContext()
        {
        }

        public BadMelonDataContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<IngredientType> IngredientTypes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Step> Steps { get; set; }

        public async Task Seed()
        {
            await Database.EnsureCreatedAsync();
            var data = new DataSamples();
            await IngredientTypes.AddRangeAsync(data.IngredientTypes);
            await Recipes.AddRangeAsync(data.Recipes);
            await SaveChangesAsync();
        }
    }
}